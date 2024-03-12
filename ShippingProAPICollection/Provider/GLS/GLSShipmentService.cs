using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Provider.GLS.Entities;
using ShippingProAPICollection.Provider.GLS.Entities.Cancel;
using ShippingProAPICollection.Provider.GLS.Entities.Create;
using ShippingProAPICollection.Provider.GLS.Entities.Create.Response;
using ShippingProAPICollection.Provider.GLS.Entities.Create.Services;
using ShippingProAPICollection.Provider.GLS.Entities.EstimatedDeliveryDays;
using ShippingProAPICollection.Provider.GLS.Entities.Validation;

namespace ShippingProAPICollection.Provider.GLS
{
    public class GLSShipmentService : IShippingProviderService
    {
        private string apiContentType = "application/glsVersion1+json";
        private GLSSettings providerSettings = null!;
        private ShippingProAPIAccountSettings accountSettings = null!;

        public GLSShipmentService(ShippingProAPIAccountSettings accountSettings, GLSSettings providerSettings)
        {
            this.accountSettings = accountSettings;
            this.providerSettings = providerSettings;
        }

        public async Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken cancelToken = default)
        {
            var GLSRequest = request as GLSShipmentRequestModel;

            PrintingOptions printOptions = new PrintingOptions()
            {
                ReturnLabels = new ReturnLabels()
                {
                    LabelFormat = GLSLabelDocFormat.PDF,
                    TemplateSet = GLSTemplateSet.NONE
                }
            };

            var shipment = CreateRequestModel(GLSRequest);

            var shipmentRequest = new ShipmentRequestData()
            {
                Shipment = shipment,
                PrintingOptions = printOptions
            };

            RestResponse<CreatedShipmentResponse> response = await CallApi<CreatedShipmentResponse>(
                new Uri(string.Format("https://shipit-wbm-{0}.gls-group.eu:443/backend/rs/shipments", providerSettings.ApiDomain)),
                Method.Post,
                shipmentRequest,
                cancelToken
                );

            List<RequestShippingLabelResponse> createdLabels = new List<RequestShippingLabelResponse>();

            for (int i = 0; i < response.Data.CreatedShipment.ParcelData.Count(); i++)
            {
                createdLabels.Add(new RequestShippingLabelResponse()
                {
                    CancelId = response.Data.CreatedShipment.ParcelData[i].TrackID,
                    ParcelNumber = response.Data.CreatedShipment.ParcelData[i].ParcelNumber,
                    Label = response.Data.CreatedShipment.PrintData[i].Data,
                    LabelType = GLSRequest.ServiceType == GLSServiceType.SHOPRETURN ? ShippingLabelType.SHOPRETURN : (request.IsExpress() ? ShippingLabelType.EXPRESS : ShippingLabelType.NORMAL),
                    Weight = request.GetPackageWeight()
                });
            }

            return createdLabels;

        }

        public async Task<ShippingCancelResult> CancelLabel(string cancelId, CancellationToken cancelToken = default)
        {
            RestResponse<CancelShipmentResponse> response = await CallApi<CancelShipmentResponse>(
                new Uri(string.Format("https://shipit-wbm-{0}.gls-group.eu:443/backend/rs/shipments/cancel/{1}", providerSettings.ApiDomain, cancelId)),
                Method.Post,
                cancelId,
                cancelToken
                );

            switch (response.Data.Result)
            {
                case "CANCELLED":
                case "CANCELLATION_PENDING":
                    return ShippingCancelResult.CANCLED;
                case "SCANNED":
                    return ShippingCancelResult.ALREADY_IN_USE;
                default:
                    throw new GLSException(ShippingErrorCode.UNKNOW, "Unknow cancel reponse", new { payload = cancelId, respone = response.Data.Result });
            }

        }

        public async Task<ValidationReponse> ValidateLabel(RequestShipmentBase request, CancellationToken cancelToken)
        {
            var GLSRequest = request as GLSShipmentRequestModel;

            var shipment = CreateRequestModel(GLSRequest);

            var requestBody = new ValidateShipmentRequestData() { Shipment = shipment };

            RestResponse<ValidateParcelsResponse> response = await CallApi<ValidateParcelsResponse>(
               new Uri(string.Format("https://shipit-wbm-{0}.gls-group.eu:443/backend/rs/shipments/validate", providerSettings.ApiDomain)),
               Method.Post,
               requestBody,
               cancelToken
               );


            if (response.Data.Success == true) return new ValidationReponse() { Success = true };
           
            List<ValidationReponseIssue> reponseIssues = new List<ValidationReponseIssue>();
            Dictionary<string, ValidationIssue> validationErrors = new Dictionary<string, ValidationIssue>();

            response.Data.ValidationResult?.Issues?.ForEach(x => validationErrors.Add(x.Rule ?? "", x));

            foreach (KeyValuePair<string, ValidationIssue> validationErrorKey in validationErrors)
            {
                switch (validationErrorKey.Key)
                {
                    case "SHIPMENT_VALID_INCOTERM_IF_NEEDED":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_INCOTERM_ERROR, Message = "Es wird ein gültiger Incoterm für die Sendung benötigt." });
                        break;
                    case "ARTICLES_PRODUCT_MUST_BE_SET":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_PRODUCT_CANNOT_USE_ERROR, Message = "GLS-Produkt kann für die Lieferadresse nicht angewandt werden." });
                        break;
                    case "ARTICLES_EXPRESS_SATURDAY":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_NEXT_DAY_NOT_SATURDAY_ERROR, Message = "Nächster Werktag ist nicht Samstag. Service kann heute nicht gebucht werden." });
                        break;
                    case "SHIPMENT_VALID_ROUTING":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_ROUTING_ERROR, Message = "Kein gültiges Routing mit GLS möglich: " + validationErrorKey.Value.Parameters[0] });
                        break;
                    case "ADDRESS_VALID_ZIPCODE":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_POSTCODE_ERROR, Message = "Keine gültige Postleitzahl vorhanden." });
                        break;
                    case "ARTICLES_PRODUCT_WEIGHT":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_WEIGHT_ERROR, Message = "Gewicht ist zu gering oder zu hoch für dieses Produkt." });
                        break;
                    case "ARTICLE_COMBINATIONS":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_ARTICLE_COMBINATIONS_ERROR, Message = "GLS-Produktkombination können nicht zusammen gebucht werden." });
                        break;
                    case "ARTICLES_DESTINATION_EXCLUSIONS":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_ARTICLE_DESTINATION_EXCLUSION_ERROR, Message = "GLS-Service und Produkt sind zur Lieferadresse nicht möglich" });
                        break;
                    case "COMMON":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_COMMON_ERROR, Message = $"GLS-Labeldruck einfacher Fehler aufgetreten: Location: {validationErrorKey.Value.Location} Message: {validationErrorKey.Value.Parameters[0]}" });
                        break;
                    case "ARTICLES_VALID_FOR_COUNTRY":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_ARTICLE_COMBINATIONS_ERROR, Message = $"Artikelkombination ist in diesem Land nicht verfügbar" });
                        break;
                    default:
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ShippingErrorCode.UNKNOW, Message = "GLS-Labeldruck nicht abgedeckter Fehler entdeckt: " + validationErrorKey.Key });
                        break;
                }
            }

            return new ValidationReponse() { Success = true, ValidationIssues = reponseIssues };

        }

        public async Task<uint> GetEstimatedDeliveryDays(RequestShipmentBase request, CancellationToken cancelToken)
        {
           
            EstimatedDeliveryDaysAddress senderAddress = new EstimatedDeliveryDaysAddress()
            {
                City = accountSettings.City,
                CountryCode = accountSettings.CountryIsoA2Code,
                ZIPCode = accountSettings.PostCode,
                Street = accountSettings.Street,
            };
            senderAddress.Validate();

            EstimatedDeliveryDaysAddress destinationAddress = new EstimatedDeliveryDaysAddress()
            {
                City = request.City,
                CountryCode = request.Country,
                ZIPCode = request.PostCode,
                Street = request.Street,
                StreetNumber = request.StreetNumber,
            };
            destinationAddress.Validate();

            var requestBody = new
            {
                Source = new
                {
                    Address = senderAddress,
                },
                Destination = new
                {
                    Address = destinationAddress,
                }
            };

            RestResponse<EstimatedDeliveryDaysResponse> response = await CallApi<EstimatedDeliveryDaysResponse>(
              new Uri(string.Format("https://shipit-wbm-{0}.gls-group.eu:443/backend/rs/timeframe/deliverydays", providerSettings.ApiDomain)),
              Method.Post,
              requestBody,
              cancelToken
              );

            return 0;
        }


        /// <summary>
        /// Call GLS API and check HTTP Reponse
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="body"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        private async Task<RestResponse<T>> CallApi<T>(Uri url, Method method, object body, CancellationToken cancelToken)
        {
            var clientOptions = new RestClientOptions(url)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                Authenticator = new HttpBasicAuthenticator(providerSettings.Username, providerSettings.Password)
            };

            RestClient client = new RestClient(clientOptions);

            var clientRequest = new RestRequest()
            {
                Method = method
            };

            clientRequest.AddHeader("Content-Type", apiContentType);
            clientRequest.AddHeader("Accept", apiContentType);

            string json = JsonConvert.SerializeObject(body, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });
            clientRequest.AddJsonBody(json, apiContentType);

            var response = await client.ExecuteAsync<T>(clientRequest, cancelToken).ConfigureAwait(false);

            CheckHttpResponse<T>(json, response);

            return response;

        }

        /// <summary>
        /// Check the http reponse status
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <param name="response"></param>
        /// <exception cref="GLSException"></exception>
        private void CheckHttpResponse<T>(object payload, RestResponse<T> response)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //var glsErrorCode = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("error"))?.Value?.ToString() ?? "Unknow";
                var message = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("message"))?.Value?.ToString() ?? "Unknow";

                ShippingErrorCode errorCode = ShippingErrorCode.UNKNOW;

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    errorCode = ShippingErrorCode.BAD_REQUEST_ERROR;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    errorCode = ShippingErrorCode.UNAUTHORIZED;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    errorCode = ShippingErrorCode.INTERNAL_SERVER_ERROR;
                }
                else
                {
                    throw new GLSException(ShippingErrorCode.UNKNOW, response.ErrorMessage + "<------>" + response.Content, payload);
                }
                 
                throw new GLSException(errorCode, message, payload);
            }

            if (response.Data == null) throw new GLSException(ShippingErrorCode.UNKNOW, "No Data available in response", payload);
        }

        /// <summary>
        /// Create the GLS request body informations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Shipment CreateRequestModel(GLSShipmentRequestModel request)
        {

            float packageWeight = request.GetPackageWeight();

            List<ShipmentUnit> units = new List<ShipmentUnit>();

            List<string> shipmentUnitReference = new List<string>();

            if (!String.IsNullOrEmpty(request.AmazonOrderId))
            {
                shipmentUnitReference.Add(request.AmazonOrderId);
            }

            if (!String.IsNullOrEmpty(request.InvoiceReference))
            {
                shipmentUnitReference.Add("INr: " + request.InvoiceReference);
            }

            if (!String.IsNullOrEmpty(request.CustomerReference))
            {
                shipmentUnitReference.Add("KNr: " + request.CustomerReference);
            }

            // Create shipment units
            for (int i = 0; i < request.LabelCount; i++)
            {
                units.Add(new ShipmentUnit()
                {
                    Weight = Convert.ToDecimal(packageWeight),
                    Note1 = request.Note1 ?? "",
                    Note2 = request.Note2 ?? "",
                    ShipmentUnitReference = shipmentUnitReference.ToArray(),
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
                        EMail = (request.WithEmailNotification || !String.IsNullOrEmpty(request.AmazonOrderId))  ? request.EMail : null,
                        MobilePhoneNumber = request.Phone,
                    }
                },

                Shipper = new Shipper()
                {
                    ContactID = providerSettings.ContactID,
                    AlternativeShipperAddress = null
                },

                ShipmentUnit = units.ToArray(),
                Service = ResolveGLSShipmentService(request),
            };


            return shipment;
           
        }

        /// <summary>
        /// Build GLS shipping service
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private ShipmentService[]? ResolveGLSShipmentService(GLSShipmentRequestModel request)
        {
            if (request.ServiceType == GLSServiceType.NONE) return null;

            ShipmentService service = new ShipmentService();

            switch (request.ServiceType)
            {
                case GLSServiceType.DEPOSIT:
                    service.Deposit = new DepositService() { PlaceOfDeposit = request.PlaceOfDeposit ?? "" };
                    break;
                case GLSServiceType.G24:
                    service.Service = new Guaranteed24Service();
                    break;
                case GLSServiceType.G8:
                    service.Service = new Service0800();
                    break;
                case GLSServiceType.G9:
                    service.Service = new Service0900();
                    break;
                case GLSServiceType.G10:
                    service.Service = new Service1000();
                    break;
                case GLSServiceType.G12:
                    service.Service = new Service1200();
                    break;
                case GLSServiceType.GSATURDAY10:
                    service.Service = new Saturday1000Service();
                    break;
                case GLSServiceType.GSATURDAY12:
                    service.Service = new Saturday1200Service();
                    break;
                case GLSServiceType.SHOPRETURN:
                    service.ShopReturn = new ShopReturnService() { NumberOfLabels = request.LabelCount };
                    break;
                default:
                    break;
            }

            return new ShipmentService[] { service };
        }

    }
}
