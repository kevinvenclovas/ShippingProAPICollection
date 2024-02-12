using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using ShippingProAPICollection._Provider.DHL;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
using ShippingProAPICollection.Provider.DHL.Entities;
using ShippingProAPICollection.Provider.ShipIT.Entities.Validation;

namespace ShippingProAPICollection.Provider.DHL
{
    public class DHLShipmentService : IShippingProviderService
    {

        private DHLSettings providerSettings = null!;
        private ShippingProAPIAccountSettings accountSettings = null!;

        public DHLShipmentService(ShippingProAPIAccountSettings accountSettings, DHLSettings providerSettings)
        {
            this.accountSettings = accountSettings;
            this.providerSettings = providerSettings;
        }

        /// <summary>
        /// Request DHL shipping label
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="DHLException"></exception>
        public async Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken cancelToken = default)
        {
            var DHLRequest = request as DHLShipmentRequestModel;

            var urlBuilder_ = new System.Text.StringBuilder();
            var baseUrl = string.Format("https://api-{0}.dhl.com/parcel/de/shipping/v2/", providerSettings.ApiDomain);

            urlBuilder_.Append(baseUrl != null ? baseUrl.TrimEnd('/') : "").Append("/orders?");
            urlBuilder_.Append(Uri.EscapeDataString("docFormat") + "=").Append(DHLLabelDocFormat.PDF.ToString()).Append("&");
            urlBuilder_.Append(Uri.EscapeDataString("printFormat") + "=").Append(providerSettings.LabelPrintFormat).Append("&");
            urlBuilder_.Length--;

            var clientOptions = new RestClientOptions(new Uri(urlBuilder_.ToString(), UriKind.RelativeOrAbsolute))
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                Authenticator = new HttpBasicAuthenticator(providerSettings.Username, providerSettings.Password)
            };

            using (RestClient client = new RestClient(clientOptions))
            {

                var clientRequest = new RestRequest()
                {
                    Method = Method.Post
                };

                clientRequest.AddHeader("dhl-api-key", providerSettings.APIKey);
                clientRequest.AddHeader("Content-Type", "application/json");

                if (!string.IsNullOrEmpty(providerSettings.APILanguage))
                {
                    clientRequest.AddHeader("Accept-Language", providerSettings.APILanguage);
                };

                var requestBody = CreateRequestModel(DHLRequest);

                string requestJson = JsonConvert.SerializeObject(requestBody, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                });

                clientRequest.AddBody(requestJson);

                RestResponse response = await client.ExecuteAsync(clientRequest, cancelToken);

                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.MultiStatus)
                {
                    var objectResponse_ = JsonConvert.DeserializeObject<LabelDataResponse>(response.Content);
                    if (objectResponse_ == null)
                    {
                        throw new DHLException(ErrorCode.CANNOT_CONVERT_RESPONSE, "Cannot convert reponse to object", new { payload = requestBody, respone = response.Content });
                    }

                    List<RequestShippingLabelResponse> labels = new List<RequestShippingLabelResponse>();

                    foreach (ResponseItem c in objectResponse_.Items)
                    {
                        labels.Add(new RequestShippingLabelResponse()
                        {
                            Label = Convert.FromBase64String(c.Label.B64),
                            ParcelNumber = c.ShipmentNo,
                            CancelId = c.ShipmentNo
                        });
                    }

                    return labels;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var objectResponse_ = JsonConvert.DeserializeObject<LabelDataResponse>(response.Content);
                    if (objectResponse_ == null)
                    {
                        throw new DHLException(ErrorCode.CANNOT_CONVERT_RESPONSE, "Cannot convert reponse to object", new { payload = requestBody, respone = response.Content });
                    }

                    throw new DHLException(ErrorCode.BAD_REQUEST_ERROR, objectResponse_.Items?.FirstOrDefault()?.ValidationMessages.FirstOrDefault()?.ValidationMessage ?? "Unknow message", new { payload = requestBody, respone = response.Content });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new DHLException(ErrorCode.UNAUTHORIZED, $"Request reponsed with HTTP status code {(int)response.StatusCode}", new { payload = requestBody, respone = response.Content });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    throw new DHLException(ErrorCode.TO_MANY_REQUESTS, $"Request reponsed with HTTP status code {(int)response.StatusCode}", new { payload = requestBody, respone = response.Content });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new DHLException(ErrorCode.INTERNAL_SERVER_ERROR, $"Request reponsed with HTTP status code {(int)response.StatusCode}", new { payload = requestBody, respone = response.Content });
                }
                else
                {
                    throw new DHLException(ErrorCode.UNKNOW, "The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", new { payload = requestBody, respone = response.Content });
                }

            }

        }

        /// <summary>
        /// Cancel DHL shipping label
        /// </summary>
        /// <param name="cancelId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="DHLException"></exception>
        public async Task<CancelResult> CancelLabel(string cancelId, CancellationToken cancelToken = default)
        {
            var urlBuilder_ = new System.Text.StringBuilder();
            var baseUrl = string.Format("https://api-{0}.dhl.com/parcel/de/shipping/v2/", providerSettings.ApiDomain);

            urlBuilder_.Append(baseUrl != null ? baseUrl.TrimEnd('/') : "").Append("/orders?");
            urlBuilder_.Append(Uri.EscapeDataString("profile") + "=").Append(providerSettings.DHLShipmentProfile).Append("&");
            urlBuilder_.Append(Uri.EscapeDataString("shipment") + "=").Append(cancelId).Append("&");
            urlBuilder_.Length--;

            var clientOptions = new RestClientOptions(new Uri(urlBuilder_.ToString(), UriKind.RelativeOrAbsolute))
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                Authenticator = new HttpBasicAuthenticator(providerSettings.Username, providerSettings.Password)
            };

            using (RestClient client = new RestClient(clientOptions))
            {

                var clientRequest = new RestRequest()
                {
                    Method = Method.Delete
                };

                clientRequest.AddHeader("dhl-api-key", providerSettings.APIKey);
                clientRequest.AddHeader("Content-Type", "application/json");

                if (!string.IsNullOrEmpty(providerSettings.APILanguage))
                {
                    clientRequest.AddHeader("Accept-Language", providerSettings.APILanguage);
                };

                RestResponse response = await client.ExecuteAsync(clientRequest, cancelToken);

                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.MultiStatus)
                {
                    var objectResponse_ = JsonConvert.DeserializeObject<LabelDataResponse>(response.Content);
                    if (objectResponse_ == null)
                    {
                        throw new DHLException(ErrorCode.CANNOT_CONVERT_RESPONSE, "Cannot convert reponse to object", new { payload = cancelId, respone = response.Content });
                    }

                    return CancelResult.CANCLED;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var objectResponse_ = JsonConvert.DeserializeObject<LabelDataResponse>(response.Content);
                    if (objectResponse_ == null)
                    {
                        throw new DHLException(ErrorCode.CANNOT_CONVERT_RESPONSE, "Cannot convert reponse to object", new { payload = cancelId, respone = response.Content });
                    }

                    throw new DHLException(ErrorCode.BAD_REQUEST_ERROR, objectResponse_.Items?.FirstOrDefault()?.ValidationMessages.FirstOrDefault()?.ValidationMessage ?? "Unknow message", new { payload = cancelId, respone = response.Content });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new DHLException(ErrorCode.UNAUTHORIZED, $"Request reponsed with HTTP status code {(int)response.StatusCode}", new { payload = cancelId, respone = response.Content });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    throw new DHLException(ErrorCode.TO_MANY_REQUESTS, $"Request reponsed with HTTP status code {(int)response.StatusCode}", new { payload = cancelId, respone = response.Content });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new DHLException(ErrorCode.INTERNAL_SERVER_ERROR, $"Request reponsed with HTTP status code {(int)response.StatusCode}", new { payload = cancelId, respone = response.Content });
                }
                else
                {
                    throw new DHLException(ErrorCode.UNKNOW, "The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", new { payload = cancelId, respone = response.Content });
                }
            }

        }

        /// <summary>
        /// Validate a shipping label request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public Task<ValidationReponse> ValidateLabel(RequestShipmentBase request, CancellationToken cancelToken)
        {
            throw new DHLException(ErrorCode.NOT_AVAILABLE, "Validation not available for DHL");
        }

        /// <summary>
        /// Create the DHL request body informations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private ShipmentOrderRequest CreateRequestModel(DHLShipmentRequestModel request)
        {

            float packageWeight = request.Weight / request.LabelCount;
            packageWeight = (float)Math.Round(packageWeight, 2);

            // Build shipper infos
            Shipper shipper = new Shipper()
            {
                Name1 = accountSettings.Name,
                Name2 = accountSettings.Name2,
                Name3 = accountSettings.Name3,
                AddressStreet = accountSettings.Street,
                PostalCode = accountSettings.PostCode,
                City = accountSettings.City,
                Country = EnumUtils.ToEnum(ThreeLetterCountryCodeHelper.GetThreeLetterCountryCode(accountSettings.CountryIsoA2Code), Country.UNKNOWN),
                ContactName = accountSettings.ContactName,
                Email = accountSettings.Email,
            };

            Consignee consignee = GetConsignee(request);

            ShipmentOrderRequest shipmentOrderRequest = new ShipmentOrderRequest();
            shipmentOrderRequest.Profile = providerSettings.DHLShipmentProfile;

            // Build shipment for each requested label
            for (int i = 0; i < request.LabelCount; i++)
            {
                Shipment shipment = new Shipment();

                shipment.Product = request.ServiceProduct.ToString();
                shipment.BillingNumber = GetBillingNumber(request);
                shipment.ShipDate = DateTimeOffset.Now;
                shipment.Shipper = shipper;
                shipment.Consignee = consignee;
                shipment.RefNo = request.InvoiceReference ?? null;

                shipment.Details = new ShipmentDetails()
                {
                    Weight = new Weight()
                    {
                        Uom = WeightUom.Kg,
                        Value = packageWeight
                    }
                };

                shipment.Services = new VAS()
                {
                    PreferredLocation = request.ServiceType == DHLServiceType.DEPOSIT ? request.PlaceOfDeposit : null,
                };

                shipmentOrderRequest.Shipments.Add(shipment);
            }

            return shipmentOrderRequest;

        }

        /// <summary>
        /// Build consignee informations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Consignee GetConsignee(DHLShipmentRequestModel request)
        {
            Consignee consignee = new Consignee();
            consignee.AdditionalProperties.Add("city", request.City);
            consignee.AdditionalProperties.Add("postalCode", request.PostCode);

            if (request.WithEmailNotification && !string.IsNullOrEmpty(request.EMail))
            {
                consignee.AdditionalProperties.Add("email", request.EMail);
            }

            switch (request.ServiceType)
            {
                case DHLServiceType.NONE:
                case DHLServiceType.DEPOSIT:
                    consignee.AdditionalProperties.Add("name1", request.Adressline1);
                    consignee.AdditionalProperties.Add("name2", request.Adressline2);
                    consignee.AdditionalProperties.Add("name3", request.Adressline3);
                    consignee.AdditionalProperties.Add("dispatchingInformation", request.Note1 ?? null);
                    consignee.AdditionalProperties.Add("addressStreet", request.Street);
                    consignee.AdditionalProperties.Add("addressHouse", request.StreetNumber);
                    consignee.AdditionalProperties.Add("country", ThreeLetterCountryCodeHelper.GetThreeLetterCountryCode(request.Country));
                    consignee.AdditionalProperties.Add("phone", request.Phone);
                    break;
                case DHLServiceType.LOCKER:
                    consignee.AdditionalProperties.Add("name", request.Adressline1);
                    consignee.AdditionalProperties.Add("lockerID", request.Locker.PackstationNumber);
                    consignee.AdditionalProperties.Add("postNumber", request.Locker.PostNumber);
                    break;
                case DHLServiceType.POSTOFFICE:
                    consignee.AdditionalProperties.Add("name", request.Adressline1);
                    consignee.AdditionalProperties.Add("retailID", request.PostOffice.PostfilialeNumber);
                    consignee.AdditionalProperties.Add("postNumber", request.PostOffice.PostNumber);
                    break;
            }

            return consignee;
        }

        /// <summary>
        /// Get DHL billing number by product type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string GetBillingNumber(DHLShipmentRequestModel request)
        {
            switch (request.ServiceProduct)
            {
                case DHLProductType.V01PAK:
                case DHLProductType.V01PRIO:
                    return providerSettings.NationalAccountNumber;
                case DHLProductType.V53WPAK:
                    return providerSettings.InternationalAccountNumber;
                default:
                    throw new Exception("Accountnumber not defined");
            }
        }
    }
}
