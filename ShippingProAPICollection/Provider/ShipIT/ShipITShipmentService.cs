using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Provider;
using ShippingProAPICollection.Provider.ShipIT;
using ShippingProAPICollection.Provider.ShipIT.Entities;
using ShippingProAPICollection.Provider.ShipIT.Entities.Cancel;
using ShippingProAPICollection.Provider.ShipIT.Entities.Create;
using ShippingProAPICollection.Provider.ShipIT.Entities.Create.Response;
using ShippingProAPICollection.Provider.ShipIT.Entities.Create.Services;


namespace ShippingProAPICollection.Provider.ShipIT
{
    public class ShipITShipmentService : IShippingProviderService
    {
        private string apiContentType = "application/glsVersion1+json";
        private ShipITSettings providerSettings = null!;
        private ShippingProAPIAccountSettings accountSettings = null!;

        public ShipITShipmentService(ShippingProAPIAccountSettings accountSettings, ShipITSettings providerSettings)
        {
            this.accountSettings = accountSettings;
            this.providerSettings = providerSettings;
        }

        /// <summary>
        /// Request GLS Shipping label
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="ShipITException"></exception>
        public async Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken cancelToken = default)
        {
            var shipITRequest = request as ShipITShipmentRequestModel;

            var requestBody = CreateRequestModel(shipITRequest);

            var clientOptions = new RestClientOptions(new Uri(string.Format("https://shipit-wbm-{0}.gls-group.eu:443/backend/rs/shipments", providerSettings.ApiDomain)))
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                Authenticator = new HttpBasicAuthenticator(providerSettings.Username, providerSettings.Password)
            };

            RestClient client = new RestClient(clientOptions);

            var clientRequest = new RestRequest()
            {
                Method = Method.Post
            };

            clientRequest.AddHeader("Content-Type", apiContentType);
            clientRequest.AddHeader("Accept", apiContentType);

            string json = JsonConvert.SerializeObject(requestBody, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });

            clientRequest.AddJsonBody(json, apiContentType);

            RestResponse<CreatedShipmentResponse> response = await client.ExecuteAsync<CreatedShipmentResponse>(clientRequest, cancelToken).ConfigureAwait(false);

            if (response.Data == null) throw new ShipITException(ErrorCode.UNKNOW, "No Data available in response", requestBody);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //var glsErrorCode = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("error"))?.Value?.ToString() ?? "Unknow";
                var message = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("message"))?.Value?.ToString() ?? "Unknow";

                ErrorCode errorCode = ErrorCode.UNKNOW;

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    errorCode = ErrorCode.BAD_REQUEST_ERROR;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    errorCode = ErrorCode.UNAUTHORIZED;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    errorCode = ErrorCode.TO_MANY_REQUESTS;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    errorCode = ErrorCode.INTERNAL_SERVER_ERROR;
                }

                throw new ShipITException(errorCode, message, requestBody);
            }

            List<RequestShippingLabelResponse> createdLabels = new List<RequestShippingLabelResponse>();

            for (int i = 0; i < response.Data.CreatedShipment.ParcelData.Count(); i++)
            {
                createdLabels.Add(new RequestShippingLabelResponse()
                {
                    CancelId = response.Data.CreatedShipment.ParcelData[i].TrackID,
                    ParcelNumber = response.Data.CreatedShipment.ParcelData[i].ParcelNumber,
                    Label = response.Data.CreatedShipment.PrintData[i].Data,
                });
            }

            return createdLabels;

        }

        /// <summary>
        /// Cancel GLS shipping label
        /// </summary>
        /// <param name="cancelId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="ShipITException"></exception>
        public async Task<CancelResult> CancelLabel(string cancelId, CancellationToken cancelToken = default)
        {

            var clientOptions = new RestClientOptions(new Uri(string.Format("https://shipit-wbm-{0}.gls-group.eu:443/backend/rs/shipments/cancel/{1}", providerSettings.ApiDomain, cancelId)))
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                Authenticator = new HttpBasicAuthenticator(providerSettings.Username, providerSettings.Password)
            };

            RestClient client = new RestClient(clientOptions);

            var clientRequest = new RestRequest()
            {
                Method = Method.Post
            };

            clientRequest.AddHeader("Content-Type", apiContentType);
            clientRequest.AddHeader("Accept", apiContentType);

            RestResponse<CancelShipmentResponse> response = await client.ExecuteAsync<CancelShipmentResponse>(clientRequest, cancelToken).ConfigureAwait(false);

            if (response.Data == null) throw new ShipITException(ErrorCode.UNKNOW, "No Data available in response", cancelId);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //var glsErrorCode = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("error"))?.Value?.ToString() ?? "Unknow";
                var message = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("message"))?.Value?.ToString() ?? "Unknow";

                ErrorCode errorCode = ErrorCode.UNKNOW;

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    errorCode = ErrorCode.BAD_REQUEST_ERROR;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    errorCode = ErrorCode.UNAUTHORIZED;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    errorCode = ErrorCode.TO_MANY_REQUESTS;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    errorCode = ErrorCode.INTERNAL_SERVER_ERROR;
                }

                throw new ShipITException(errorCode, message, new { payload = cancelId, respone = response });
            }

            switch (response.Data.Result)
            {
                case "CANCELLED":
                case "CANCELLATION_PENDING":
                    return CancelResult.CANCLED;
                case "SCANNED":
                    return CancelResult.ALREADY_IN_USE;
                default:
                    throw new ShipITException(ErrorCode.UNKNOW, "Unknow cancel reponse", new { payload = cancelId, respone = response.Data.Result });
            }

        }



        /// <summary>
        /// Create the GLS request body informations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private ShipmentRequestData CreateRequestModel(ShipITShipmentRequestModel request)
        {
            // Define printing options
            PrintingOptions printOptions = new PrintingOptions();

            printOptions.ReturnLabels = new ReturnLabels()
            {
                LabelFormat = ShipITLabelDocFormat.PDF,
                TemplateSet = ShipITTemplateSet.NONE
            };

            // Calculate single package weight
            // Share weight if more than one label requested
            double singlePackageWeight = request.Weight / request.LabelCount;
            // Minimum weight 1 Kg
            if (singlePackageWeight < 1) singlePackageWeight = 1;

            List<ShipmentUnit> units = new List<ShipmentUnit>();

            // Create shipment units
            for (int i = 0; i < request.LabelCount; i++)
            {
                units.Add(new ShipmentUnit()
                {
                    Weight = Convert.ToDecimal(singlePackageWeight),
                    Note1 = request.Note1 ?? "",
                    Note2 = request.Note2 ?? "",
                    ShipmentUnitReference = string.IsNullOrEmpty(request.ShipmentReference) ? null : new List<string>() { request.ShipmentReference }.ToArray(),
                });
            }

            string? incotermCode = request.IncotermCode == null || request.IncotermCode != null && request.IncotermCode == 0 ? null : request.IncotermCode.ToString();

            // Create shipment
            Shipment shipment = new Shipment()
            {
                IncotermCode = incotermCode,
                ShippingDate = request.EarliestDeliveryDate.ToString("yyyy-MM-dd"),
                Product = request.ServiceProduct,
                Consignee = new Consignee()
                {
                    Address = new Address()
                    {
                        Name1 = request.Adressline1,
                        Name2 = request.Adressline2,
                        Name3 = request.Adressline3,
                        CountryCode = request.Country,
                        ZIPCode = request.PostCode,
                        City = request.City,
                        Street = request.Street,
                        StreetNumber = request.StreetNumber ?? "-",
                        EMail = request.WithEmailNotification ? request.EMail : null,
                        MobilePhoneNumber = request.Phone,
                    }
                },

                Shipper = new Shipper()
                {
                    ContactID = providerSettings.ContactID,
                    AlternativeShipperAddress = null
                },

                ShipmentUnit = units.ToArray(),
                Service = ResolveShipITShipmentService(request),
            };


            return new ShipmentRequestData()
            {
                Shipment = shipment,
                PrintingOptions = printOptions
            };

        }

        /// <summary>
        /// Build GLS shipping service
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private ShipmentService[]? ResolveShipITShipmentService(ShipITShipmentRequestModel request)
        {
            if (request.ServiceType == ShipITServiceType.NONE) return null;

            ShipmentService service = new ShipmentService();

            switch (request.ServiceType)
            {
                case ShipITServiceType.DEPOSIT:
                    service.Deposit = new DepositService() { PlaceOfDeposit = request.PlaceOfDeposit ?? "" };
                    break;
                case ShipITServiceType.G24:
                    service.Service = new Guaranteed24Service();
                    break;
                case ShipITServiceType.G8:
                    service.Service = new Service0800();
                    break;
                case ShipITServiceType.G9:
                    service.Service = new Service0900();
                    break;
                case ShipITServiceType.G10:
                    service.Service = new Service1000();
                    break;
                case ShipITServiceType.G12:
                    service.Service = new Service1200();
                    break;
                case ShipITServiceType.GSATURDAY10:
                    service.Service = new Saturday1000Service();
                    break;
                case ShipITServiceType.GSATURDAY12:
                    service.Service = new Saturday1200Service();
                    break;
                case ShipITServiceType.SHOPRETURN:
                    service.ShopReturn = new ShopReturnService() { NumberOfLabels = request.LabelCount };
                    break;
                default:
                    break;
            }

            return new ShipmentService[] { service };
        }

    }
}
