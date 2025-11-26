using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Authenticators;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
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
        private ShippingProAPIShipFromAddress defaultShipFromAddress = null!;

        public GLSShipmentService(ShippingProAPIShipFromAddress defaultShipFromAddress, GLSSettings providerSettings)
        {
            this.defaultShipFromAddress = defaultShipFromAddress;
            this.providerSettings = providerSettings;
        }

        public async Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken cancelToken = default)
        {

            var GLSRequest = request as GLSShipmentRequestModel;
            if (GLSRequest == null) throw new Exception("Cannot convert request to GLSShipmentRequestModel");

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
                new Uri(string.Format("{0}/backend/rs/shipments", providerSettings.ApiDomain)),
                Method.Post,
                shipmentRequest,
                cancelToken
                );

            List<RequestShippingLabelResponse> createdLabels = new List<RequestShippingLabelResponse>();

            if (response.Data != null)
            {
                for (int i = 0; i < response.Data.CreatedShipment.ParcelData.Count(); i++)
                {
                    createdLabels.Add(new RequestShippingLabelResponse()
                    {
                        CancelId = response.Data.CreatedShipment.ParcelData[i].TrackID,
                        ParcelNumber = response.Data.CreatedShipment.ParcelData[i].ParcelNumber,
                        Label = response.Data.CreatedShipment.PrintData[i].Data,
                        LabelType = GLSRequest.ServiceType == GLSServiceType.SHOPRETURN ? ShippingLabelType.SHOPRETURN : (request.IsExpress() ? ShippingLabelType.EXPRESS : ShippingLabelType.NORMAL),
                        Weight = request.Items[i].Weight
                    });
                }
            }

            return createdLabels;

        }

        public async Task<ShippingCancelResult> CancelLabel(string cancelId, CancellationToken cancelToken = default)
        {
            RestResponse<CancelShipmentResponse> response = await CallApi<CancelShipmentResponse>(
                new Uri(string.Format("{0}/backend/rs/shipments/cancel/{1}", providerSettings.ApiDomain, cancelId)),
                Method.Post,
                cancelId,
                cancelToken
                );

            if (response.Data == null) throw new Exception("Reponse was null");

            switch (response.Data.Result)
            {
                case "CANCELLED":
                case "CANCELLATION_PENDING":
                    return ShippingCancelResult.CANCELED;
                case "SCANNED":
                    return ShippingCancelResult.ALREADY_IN_USE;
                default:
                    throw new GLSException(ShippingErrorCode.UNKNOW, "Unknow cancel reponse", new { payload = cancelId, response = response.Data.Result });
            }

        }

        public async Task<ValidationReponse> ValidateLabel(RequestShipmentBase request, CancellationToken cancelToken)
        {
            var GLSRequest = request as GLSShipmentRequestModel;
            if (GLSRequest == null) throw new Exception("Cannot convert request to GLSShipmentRequestModel");

            var shipment = CreateRequestModel(GLSRequest);

            var requestBody = new ValidateShipmentRequestData() { Shipment = shipment };

            RestResponse<ValidateParcelsResponse> response = await CallApi<ValidateParcelsResponse>(
               new Uri(string.Format("{0}/backend/rs/shipments/validate", providerSettings.ApiDomain)),
               Method.Post,
               requestBody,
               cancelToken
               );

            if (response.Data == null) throw new Exception("Reponse was null");

            if (response.Data.Success == true) return new ValidationReponse() { Success = true };

            List<GLSValidationReponseIssue> reponseIssues = new List<GLSValidationReponseIssue>();
            Dictionary<string, ValidationIssue> validationErrors = new Dictionary<string, ValidationIssue>();

            response.Data.ValidationResult?.Issues?.ForEach(x => validationErrors.Add(x.Rule ?? "", x));

            foreach (KeyValuePair<string, ValidationIssue> validationErrorKey in validationErrors)
            {
                switch (validationErrorKey.Key)
                {
                    case "SHIPMENT_VALID_INCOTERM_IF_NEEDED":
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_INCOTERM_ERROR, Message = "Es wird ein gültiger Incoterm für die Sendung benötigt." });
                        break;
                    case "ARTICLES_PRODUCT_MUST_BE_SET":
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_PRODUCT_CANNOT_USE_ERROR, Message = "GLS-Produkt kann für die Lieferadresse nicht angewandt werden." });
                        break;
                    case "ARTICLES_EXPRESS_SATURDAY":
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_NEXT_DAY_NOT_SATURDAY_ERROR, Message = "Nächster Werktag ist nicht Samstag. Service kann heute nicht gebucht werden." });
                        break;
                    case "SHIPMENT_VALID_ROUTING":
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_ROUTING_ERROR, Message = "Kein gültiges Routing mit GLS möglich: " + (validationErrorKey.Value?.Parameters?[0] ?? "UNKNOW") });
                        break;
                    case "ADDRESS_VALID_ZIPCODE":
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_POSTCODE_ERROR, Message = "Keine gültige Postleitzahl vorhanden." });
                        break;
                    case "ARTICLES_PRODUCT_WEIGHT":
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_WEIGHT_ERROR, Message = "Gewicht ist zu gering oder zu hoch für dieses Produkt." });
                        break;
                    case "ARTICLE_COMBINATIONS":
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_ARTICLE_COMBINATIONS_ERROR, Message = "GLS-Produktkombination können nicht zusammen gebucht werden." });
                        break;
                    case "ARTICLES_DESTINATION_EXCLUSIONS":
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_ARTICLE_DESTINATION_EXCLUSION_ERROR, Message = "GLS-Service und Produkt sind zur Lieferadresse nicht möglich" });
                        break;
                    case "COMMON":
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_COMMON_ERROR, Message = $"GLS-Labeldruck einfacher Fehler aufgetreten: Location: {validationErrorKey.Value?.Location ?? "UNKNOW"} Message: {validationErrorKey.Value?.Parameters?[0] ?? "UNKNOW"} " });
                        break;
                    case "ARTICLES_VALID_FOR_COUNTRY":
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.GLS_ARTICLE_COMBINATIONS_ERROR, Message = $"Artikelkombination ist in diesem Land nicht verfügbar" });
                        break;
                    default:
                        reponseIssues.Add(new GLSValidationReponseIssue() { ErrorCode = ShippingErrorCode.UNKNOW, Message = "GLS-Labeldruck nicht abgedeckter Fehler entdeckt: " + validationErrorKey.Key });
                        break;
                }
            }

            return new GLSValidationReponse() { Success = true, ValidationIssues = reponseIssues };

        }

        public async Task<uint> GetEstimatedDeliveryDays(RequestShipmentBase request, CancellationToken cancelToken)
        {
            // Api NotFound on current GLS api
            return 0;

            var from = request.ShipFromAddress ?? defaultShipFromAddress;

            EstimatedDeliveryDaysAddress senderAddress = new EstimatedDeliveryDaysAddress()
            {
                City = from.City,
                CountryCode = from.CountryIsoA2Code,
                ZIPCode = from.PostCode,
                Street = from.Street,
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
                new Uri(string.Format("{0}/backend/rs/timeframe/deliverydays", providerSettings.ApiDomain)),
                Method.Post,
                requestBody,
                cancelToken
            );

            return response?.Data?.NumberOfWorkDays ?? 0;
        }

        public async Task ConfirmShipment(string parcelId, CancellationToken cancelToken)
        {
            throw new GLSException(ShippingErrorCode.NOT_AVAILABLE, "Feature not available for GLS");
        }

        public float GetMaxPackageWeight()
        {
            return providerSettings.MaxPackageWeight;
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
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = null
                }
            });

            clientRequest.AddJsonBody(json, apiContentType);

            var response = await client.ExecuteAsync<T>(clientRequest, cancelToken).ConfigureAwait(false);

            var reponseMessage = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("message"))?.Value?.ToString() ?? "Unknow";
            HTTPReponseUtils.CheckHttpResponse<GLSException>(reponseMessage, json, response);

            if (response.Data == null)
            {
                throw new GLSException(ShippingErrorCode.UNKNOW, "No Data available in response", new { payload = json, response = response.Content });
            }

            return response;

        }

        /// <summary>
        /// Create the GLS request body informations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Shipment CreateRequestModel(GLSShipmentRequestModel request)
        {

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
            for (int i = 0; i < request.Items.Count; i++)
            {
                units.Add(new ShipmentUnit()
                {
                    Weight = Convert.ToDecimal(request.Items[i].Weight),
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
                        Name1 = request.Addressline1,
                        Name2 = request.Addressline2,
                        Name3 = request.Addressline3,
                        CountryCode = request.Country,
                        ZIPCode = request.PostCode,
                        City = request.City,
                        Street = request.Street,
                        StreetNumber = request.StreetNumber ?? "-",
                        EMail = (request.WithEmailNotification || !String.IsNullOrEmpty(request.AmazonOrderId)) ? request.EMail : null,
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
            var services = new List<ShipmentService>();

            bool isExpressServiceType =
                request.ServiceProduct == GLSProductType.EXPRESS ||
                request.ServiceType == GLSServiceType.G24 ||
                request.ServiceType == GLSServiceType.G8 ||
                request.ServiceType == GLSServiceType.G9 ||
                request.ServiceType == GLSServiceType.G10 ||
                request.ServiceType == GLSServiceType.G12 ||
                request.ServiceType == GLSServiceType.GSATURDAY10 ||
                request.ServiceType == GLSServiceType.GSATURDAY12;

            if (request.WithEmailNotification && !isExpressServiceType)
            {
                services.Add(new ShipmentService { Service = new FlexDeliveryService() });
            }

            var service = request.ServiceType switch
            {
                GLSServiceType.DEPOSIT => new ShipmentService
                {
                    Deposit = new DepositService { PlaceOfDeposit = request.PlaceOfDeposit ?? string.Empty }
                },
                GLSServiceType.G24 => new ShipmentService(new Guaranteed24Service()),
                GLSServiceType.G8 => new ShipmentService(new Service0800()),
                GLSServiceType.G9 => new ShipmentService(new Service0900()),
                GLSServiceType.G10 => new ShipmentService(new Service1000()),
                GLSServiceType.G12 => new ShipmentService(new Service1200()),
                GLSServiceType.GSATURDAY10 => new ShipmentService(new Saturday1000Service()),
                GLSServiceType.GSATURDAY12 => new ShipmentService(new Saturday1200Service()),
                GLSServiceType.SHOPRETURN => new ShipmentService
                {
                    ShopReturn = new ShopReturnService { NumberOfLabels = request.Items.Count }
                },
                _ => null
            };

            if (service != null)
                services.Add(service);

            return services.ToArray();
        }


    }
}
