using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Provider.ShipIT.Entities;
using ShippingProAPICollection.Provider.ShipIT.Entities.Cancel;
using ShippingProAPICollection.Provider.ShipIT.Entities.Create;
using ShippingProAPICollection.Provider.ShipIT.Entities.Create.Response;
using ShippingProAPICollection.Provider.ShipIT.Entities.Create.Services;
using ShippingProAPICollection.Provider.ShipIT.Entities.Validation;

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

            // Define printing options
            PrintingOptions printOptions = new PrintingOptions();

            printOptions.ReturnLabels = new ReturnLabels()
            {
                LabelFormat = ShipITLabelDocFormat.PDF,
                TemplateSet = ShipITTemplateSet.NONE
            };

            var shipment = CreateRequestModel(shipITRequest);

            var shipmentRequest = new ShipmentRequestData()
            {
                Shipment = shipment,
                PrintingOptions = printOptions
            };

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

            string json = JsonConvert.SerializeObject(shipmentRequest, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });

            clientRequest.AddJsonBody(json, apiContentType);

            RestResponse<CreatedShipmentResponse> response = await client.ExecuteAsync<CreatedShipmentResponse>(clientRequest, cancelToken).ConfigureAwait(false);

            if (response.Data == null) throw new ShipITException(ErrorCode.UNKNOW, "No Data available in response", shipmentRequest);

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

                throw new ShipITException(errorCode, message, shipmentRequest);
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
        /// Validate shipping request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="ShipITException"></exception>
        public async Task<ValidationReponse> ValidateLabel(RequestShipmentBase request, CancellationToken cancelToken)
        {
            var shipITRequest = request as ShipITShipmentRequestModel;

            var shipment = CreateRequestModel(shipITRequest);

            var clientOptions = new RestClientOptions(new Uri(string.Format("https://shipit-wbm-{0}.gls-group.eu:443/backend/rs/shipments/validate", providerSettings.ApiDomain)))
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

            var requestBody = new ValidateShipmentRequestData() { Shipment = shipment };

            string json = JsonConvert.SerializeObject(new ValidateShipmentRequestData() { Shipment = shipment }, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });

            clientRequest.AddJsonBody(json, apiContentType);

            RestResponse<ValidateParcelsResponse> response = await client.ExecuteAsync<ValidateParcelsResponse>(clientRequest, cancelToken).ConfigureAwait(false);

            if (!response.IsSuccessful) throw new ShipITException(ErrorCode.UNKNOW, response.ErrorMessage + "<------>" + response.Content, requestBody);
           
            if (response.Data == null) throw new ShipITException(ErrorCode.UNKNOW, "No content data found on validation reponse");

            if (response.Data.Success == true) return new ValidationReponse() { Success = true };
           
            List<ValidationReponseIssue> reponseIssues = new List<ValidationReponseIssue>();
            Dictionary<string, ValidationIssue> validationErrors = new Dictionary<string, ValidationIssue>();

            response.Data.ValidationResult?.Issues?.ForEach(x => validationErrors.Add(x.Rule ?? "", x));

            foreach (KeyValuePair<string, ValidationIssue> validationErrorKey in validationErrors)
            {
                switch (validationErrorKey.Key)
                {
                    case "SHIPMENT_VALID_INCOTERM_IF_NEEDED":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.GLS_INCOTERM_ERROR, Message = "Es wird ein gültiger Incoterm für die Sendung benötigt." });
                        break;
                    case "ARTICLES_PRODUCT_MUST_BE_SET":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.GLS_PRODUCT_CANNOT_USE_ERROR, Message = "GLS-Produkt kann für die Lieferadresse nicht angewandt werden." });
                        break;
                    case "ARTICLES_EXPRESS_SATURDAY":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.GLS_NEXT_DAY_NOT_SATURDAY_ERROR, Message = "Nächster Werktag ist nicht Samstag. Service kann heute nicht gebucht werden." });
                        break;
                    case "SHIPMENT_VALID_ROUTING":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.GLS_ROUTING_ERROR, Message = "Kein gültiges Routing mit GLS möglich: " + validationErrorKey.Value.Parameters[0] });
                        break;
                    case "ADDRESS_VALID_ZIPCODE":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.GLS_POSTCODE_ERROR, Message = "Keine gültige Postleitzahl vorhanden." });
                        break;
                    case "ARTICLES_PRODUCT_WEIGHT":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.GLS_WEIGHT_ERROR, Message = "Gewicht ist zu gering oder zu hoch für dieses Produkt." });
                        break;
                    case "ARTICLE_COMBINATIONS":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.GLS_ARTICLE_COMBINATIONS_ERROR, Message = "GLS-Produktkombination können nicht zusammen gebucht werden." });
                        break;
                    case "ARTICLES_DESTINATION_EXCLUSIONS":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.GLS_ARTICLE_DESTINATION_EXCLUSION_ERROR, Message = "GLS-Service und Produkt sind zur Lieferadresse nicht möglich" });
                        break;
                    case "COMMON":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.GLS_COMMON_ERROR, Message = $"GLS-Labeldruck einfacher Fehler aufgetreten: Location: {validationErrorKey.Value.Location} Message: {validationErrorKey.Value.Parameters[0]}" });
                        break;
                    case "ARTICLES_VALID_FOR_COUNTRY":
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.GLS_ARTICLE_COMBINATIONS_ERROR, Message = $"Artikelkombination ist in diesem Land nicht verfügbar" });
                        break;
                    default:
                        reponseIssues.Add(new ValidationReponseIssue() { ErrorCode = ErrorCode.UNKNOW, Message = "GLS-Labeldruck nicht abgedeckter Fehler entdeckt: " + validationErrorKey.Key });
                        break;
                }
            }

            return new ValidationReponse() { Success = true, ValidationIssues = reponseIssues };

        }

        /// <summary>
        /// Create the GLS request body informations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Shipment CreateRequestModel(ShipITShipmentRequestModel request)
        {
            
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


            return shipment;
           
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
