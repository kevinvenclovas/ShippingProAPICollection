#pragma warning disable 1998

using DPDLoginService2_0;
using DPDShipmentService4_4;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
using ShippingProAPICollection.Provider.DHL.Entities;
using ShippingProAPICollection.Provider.DPD.Entities;
using ShippingProAPICollection.Provider.GLS.Entities.Validation;

namespace ShippingProAPICollection.Provider.DPD
{
    public class DPDShipmentService : IShippingProviderService
    {
        private readonly IMemoryCache _cache;
        private DPDSettings providerSettings = null!;
        private ShippingProAPIShipFromAddress defaultShipFromAddress = null!;

        public DPDShipmentService(ShippingProAPIShipFromAddress defaultShipFromAddress, DPDSettings providerSettings, IMemoryCache cache)
        {
            this.defaultShipFromAddress = defaultShipFromAddress;
            this.providerSettings = providerSettings;
            _cache = cache;
        }

        public async Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken cancelToken = default)
        {
            var DPDRequest = request as DPDShipmentRequestModel;
            if (DPDRequest == null) throw new Exception("Cannot convert request to DPDShipmentRequestModel");

            string authToken = await GetAuthToken();

            string url = string.Format("https://public-{0}.dpd.com/services/ShipmentService/V4_4/", providerSettings.ApiDomain);
            ShipmentService_4_4Client shipmentClient = new ShipmentService_4_4Client(ShipmentService_4_4Client.EndpointConfiguration.ShipmentService_Public_4_4_SOAP, url);

            storeOrders requestBody = CreateRequestModel(DPDRequest);

            authentication auth = new authentication()
            {
                delisId = providerSettings.Username,
                authToken = authToken,
                messageLanguage = providerSettings.APILanguage
            };

            storeOrdersResponse1 response;

            try
            {
                response = await shipmentClient.storeOrdersAsync(auth, requestBody).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw handleDpdErrorObject(ref ex);
            }

            List<RequestShippingLabelResponse> labels = new List<RequestShippingLabelResponse>();
            string requestId = response.storeOrdersResponse.orderResult.shipmentResponses[0].mpsId;

            foreach (shipmentResponse shipment in response.storeOrdersResponse.orderResult.shipmentResponses)
            {
                if (shipment.faults != null)
                {
                    string errorMessage = String.Join(" - ", shipment.faults.Select(x => x.faultCode + " " + x.message));
                    throw new DPDException(ShippingErrorCode.DPD_SHIPMENT_REQUEST_ERROR, errorMessage, requestBody);
                }
                else
                {
                    for (int i = 0; i < shipment.parcelInformation.Length; i++)
                    {
                        parcelInformationType parcelInfo = shipment.parcelInformation[i];

                        labels.Add(new RequestShippingLabelResponse()
                        {
                            ParcelNumber = parcelInfo.parcelLabelNumber,
                            CancelId = parcelInfo.parcelLabelNumber,
                            Label = ByteUtils.MergePDFByteToOnePDF(parcelInfo.output.Select(x => x.content).ToList()),
                            LabelType = DPDRequest.ServiceType == DPDServiceType.SHOPRETURN ? ShippingLabelType.SHOPRETURN : (request.IsExpress() ? ShippingLabelType.EXPRESS : ShippingLabelType.NORMAL),
                            Weight = request.Items[i].Weight,
                            AdditionalValues = new Dictionary<string, object> { ["REQUEST_ID"] = shipment.mpsId }
                        });
                    }
                }
            }

            return labels;

        }

        public async Task<ShippingCancelResult> CancelLabel(string cancelId, CancellationToken cancelToken = default)
        {
            // DPD is fucking crazy and we not need to cancel any labels :)
            return ShippingCancelResult.CANCELED;
        }

        public async Task<ValidationReponse> ValidateLabel(RequestShipmentBase request, CancellationToken cancelToken)
        {
            throw new DPDException(ShippingErrorCode.NOT_AVAILABLE, "Feature not available for DPD");
        }

        public async Task<uint> GetEstimatedDeliveryDays(RequestShipmentBase request, CancellationToken cancelToken)
        {
            throw new DPDException(ShippingErrorCode.NOT_AVAILABLE, "Feature not available for DPD");
        }

        public async Task ConfirmShipment(string parcelId, CancellationToken cancelToken)
        {
            throw new DHLException(ShippingErrorCode.NOT_AVAILABLE, "Feature not available for DPD");
        }

        /// <summary>
        /// Remove the cachen auth token
        /// </summary>
        public void ResetDPDAutToken()
        {
            _cache.Remove("DPD_AUTH_TOKEN_" + providerSettings.ContractID);
        }

        public float GetMaxPackageWeight()
        {
            return providerSettings.MaxPackageWeight;
        }
        private storeOrders CreateRequestModel(DPDShipmentRequestModel request)
        {
            var order = new storeOrders();

            order.printOptions = new printOptions()
            {
                printOption = new printOption[]
                {
                            new printOption()
                            {
                                paperFormat = printOptionPaperFormat.A6,
                                paperFormatSpecified = true,
                                startPosition = startPosition.UPPER_LEFT,
                                startPositionSpecified = true,
                            }
                },
                splitByParcel = true,
            };


            var shipmentServiceData = new shipmentServiceData();

            shipmentServiceData.generalShipmentData = new generalShipmentData()
            {
                mpsCustomerReferenceNumber1 = request.InvoiceReference ?? "",
                mpsCustomerReferenceNumber2 = "KNr: " + request.CustomerReference,
                mpsExpectedSendingDate = request.EarliestDeliveryDate.ToString("yyyyMMdd"),
                identificationNumber = "",
                sendingDepot = providerSettings.DepotNumber,
                product = EnumUtils.ToEnum<generalShipmentDataProduct>(request.ServiceProduct.ToString(), generalShipmentDataProduct.CL),
            };

            var from = request.ShipFromAddress ?? defaultShipFromAddress;

            shipmentServiceData.generalShipmentData.sender = new addressWithType()
            {
                addressType = addressWithTypeAddressType.COM,
                name1 = from.Name,
                name2 = from.Name2,
                email = from.Email,
                phone = from.Phone,
                street = from.Street,
                country = from.CountryIsoA2Code,
                zipCode = from.PostCode,
                city = from.City,
                contact = from.ContactName,
            };

            shipmentServiceData.generalShipmentData.recipient = new addressWithType()
            {
                addressType = request.DeliveryAdressIsCommercialCustomer ? addressWithTypeAddressType.COM : addressWithTypeAddressType.PRV,
                name1 = request.Addressline1,
                name2 = request.Addressline2,
                street = request.Street,
                houseNo = request.StreetNumber,
                country = request.Country,
                zipCode = request.PostCode,
                city = request.City,
                customerNumber = request.CustomerReference,
                phone = request.Phone,
                comment = request.Note1 ?? "",
                email = request.WithEmailNotification ? request.EMail : null,
            };

            shipmentServiceData.productAndServiceData = new productAndServiceData()
            {
                orderType = productAndServiceDataOrderType.consignment,
                predict = request.WithEmailNotification ? new notification()
                {
                    channel = 1,
                    language = request.Country,
                    value = request.EMail
                } : null,
            };

            List<parcel> parcels = new List<parcel>();

            int grammfactor = 100;

            for (int i = 0; i < request.Items.Count; i++)
            {
                parcels.Add(

                    new parcel()
                    {
                        customerReferenceNumber1 = request.InvoiceReference ?? "",
                        customerReferenceNumber2 = "KNr: " + request.CustomerReference,
                        weight = request.ServiceType == DPDServiceType.SHOPRETURN ? 1 : (int)(request.Items[i].Weight * grammfactor),
                        weightSpecified = request.ServiceType == DPDServiceType.SHOPRETURN ? false : true,
                        returns = request.ServiceType == DPDServiceType.SHOPRETURN,
                        returnsSpecified = request.ServiceType == DPDServiceType.SHOPRETURN,
                    }
                );

            }

            shipmentServiceData.parcels = parcels.ToArray();

            order.order = new shipmentServiceData[]
            {
                shipmentServiceData
            };

            return order;
        }

        /// <summary>
        /// Get auth token from cache or login into DPD service
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DPDException"></exception>
        private async Task<string> GetAuthToken()
        {
            string? dpdAuthToken = _cache.Get<string>("DPD_AUTH_TOKEN_" + providerSettings.ContractID);

            if (dpdAuthToken == null)
            {
                dpdAuthToken = await LoginToDPD();
                if (dpdAuthToken != null) _cache.Set("DPD_AUTH_TOKEN_" + providerSettings.ContractID, dpdAuthToken, TimeSpan.FromSeconds(36000));
            }

            if (String.IsNullOrEmpty(dpdAuthToken)) throw new DPDException(ShippingErrorCode.UNAUTHORIZED, "Cannot find any dpd auth token");

            return dpdAuthToken;
        }

        /// <summary>
        /// Login into DPD service
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="ShippingProviderErrorCodeException"></exception>
        private async Task<string> LoginToDPD(CancellationToken cancelToken = default)
        {

            string url = string.Format("https://public-{0}.dpd.com/services/LoginService/V2_0/", providerSettings.ApiDomain);
            LoginServiceClient loginClient = new LoginServiceClient(LoginServiceClient.EndpointConfiguration.LoginService_2_0_SOAP, url);

            getAuthResponse loginResponse;

            try
            {
                loginResponse = await loginClient.getAuthAsync(providerSettings.Username, providerSettings.Password, providerSettings.APILanguage);
            }
            catch (Exception ex)
            {
                throw handleDpdErrorObject(ref ex);
            }

            if (loginResponse.@return.authToken.Length > 0)
            {
                return loginResponse.@return.authToken;
            }
            else
            {
                throw new ShippingProviderErrorCodeException(ShippingErrorCode.UNKNOW, "No auth token available after dpd login");
            }
        }

        /// <summary>
        /// Read DPD exception from response
        /// </summary>
        /// <param name="dpdExeption"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DPDException"></exception>
        private Exception handleDpdErrorObject(ref Exception dpdExeption, object? request = null)
        {
            string getCoreErrorMessage = "";
            object? myObject = new object();
            bool FoundDetail = false;

            try
            {
                object? dpdExceptionDetail = dpdExeption.GetType().GetProperty("Detail")?.GetValue(dpdExeption, null);
                if (dpdExceptionDetail != null)
                {
                    FoundDetail = true;
                    object? errorCode = dpdExceptionDetail.GetType().GetProperty("errorCode")?.GetValue(dpdExceptionDetail, null);
                    object? errorMessage = dpdExceptionDetail.GetType().GetProperty("errorMessage")?.GetValue(dpdExceptionDetail, null);
                }

                myObject = dpdExceptionDetail;
            }
            catch (Exception)
            {
            }

            try
            {
                if (FoundDetail == true && (dpdExeption.Message.ToString() == "" | dpdExeption.Message.ToString() == "Fault occured"))
                {
                    object? dpdExceptionDetail2 = dpdExeption.GetType().GetProperty("Detail")?.GetValue(dpdExeption, null);
                    if (dpdExceptionDetail2 != null && dpdExceptionDetail2.GetType().GetProperty("InnerText")?.GetValue(dpdExceptionDetail2, null) != null)
                    {
                        object? myInnerText = dpdExceptionDetail2.GetType().GetProperty("InnerText")?.GetValue(dpdExceptionDetail2, null);
                        if (myInnerText != null && myInnerText.ToString()?.Length > 0)
                            myObject = dpdExceptionDetail2;
                    }
                }
            }
            catch (Exception)
            {
            }

            if (dpdExeption.Message.ToString().Length > 0 && !dpdExeption.Message.ToString().Contains("Fault occured"))
            {
                getCoreErrorMessage = dpdExeption.Message.ToString();
                return new DPDException(ShippingErrorCode.DPD_SHIPMENT_REQUEST_ERROR, getCoreErrorMessage, request);
            }
            else
            {
                if (myObject != null)
                {
                    System.Reflection.PropertyInfo? pi = myObject.GetType().GetProperty("errorMessage");
                    if (pi != null)
                    {
                        object? name = (object?)(pi.GetValue(myObject, null));
                        throw new DPDException(ShippingErrorCode.DPD_SHIPMENT_REQUEST_ERROR, name?.ToString() ?? "Unknow", request);
                    }
                }

                return new DPDException(ShippingErrorCode.DPD_SHIPMENT_REQUEST_ERROR, "Unknow", request);
            }

        }

    }
}
