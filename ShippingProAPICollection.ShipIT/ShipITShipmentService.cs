using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.ShipIT.Entities;
using ShippingProAPICollection.ShipIT.Entities.Cancel;
using ShippingProAPICollection.ShipIT.Entities.Create;
using ShippingProAPICollection.ShipIT.Entities.Create.Response;
using ShippingProAPICollection.ShipIT.Entities.Create.Services;


namespace ShippingProAPICollection.ShipIT
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


        public async Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken cancelToken = default)
        {
            var shipITRequest = request as ShipITShipmentRequestModel;

            #region Build request

            // Define printing options
            PrintingOptions printOptions = new PrintingOptions();

            printOptions.ReturnLabels = new ReturnLabels()
            {
                LabelFormat = providerSettings.LabelFormat,
                TemplateSet = ShipITTemplateSet.NONE
            };


            // Calculate single package weight
            // Share weight if more than one label requested
            double singlePackageWeight = shipITRequest.Weight / shipITRequest.LabelCount;
            // Minimum weight 1 Kg
            if (singlePackageWeight < 1) singlePackageWeight = 1;

            List<ShipmentUnit> units = new List<ShipmentUnit>();

            // Create shipment units
            for (int i = 0; i < shipITRequest.LabelCount; i++)
            {
                units.Add(new ShipmentUnit()
                {
                    Weight = Convert.ToDecimal(singlePackageWeight),
                    Note1 = shipITRequest.Note1 ?? "",
                    Note2 = shipITRequest.Note2 ?? "",
                    ShipmentUnitReference = String.IsNullOrEmpty(shipITRequest.ShipmentReference) ? null : new List<string>() { shipITRequest.ShipmentReference }.ToArray(),
                });
            }

            // Create shipment
            Shipment shipment = new Shipment()
            {
                IncotermCode = shipITRequest.IncotermCode?.ToString() ?? null,
                ShippingDate = shipITRequest.EarliestDeliveryDate.ToString("yyyy-MM-dd"),
                Product = shipITRequest.ServiceProduct,
                Consignee = new Consignee()
                {
                    Address = new Address()
                    {
                        Name1 = shipITRequest.Adressline1,
                        Name2 = shipITRequest.Adressline2,
                        Name3 = shipITRequest.Adressline3,
                        CountryCode = shipITRequest.Country,
                        ZIPCode = shipITRequest.ZIPCode,
                        City = shipITRequest.City,
                        Street = shipITRequest.Street,
                        StreetNumber = shipITRequest.StreetNumber ?? "-",
                        EMail = shipITRequest.WithEmailNotification ? shipITRequest.EMail : null,
                        MobilePhoneNumber = shipITRequest.Phone,
                    }
                },

                Shipper = new Shipper()
                {
                    ContactID = providerSettings.ContactID,
                    AlternativeShipperAddress = null
                },

                ShipmentUnit = units.ToArray(),
                Service = ResolveShipITShipmentService(shipITRequest),
            };


            ShipmentRequestData shipmentRequest = new ShipmentRequestData()
            {
                Shipment = shipment,
                PrintingOptions = printOptions
            };

            #endregion

            #region Send request

            var clientOptions = new RestClientOptions(new Uri(string.Format("https://shipit-wbm-{0}.gls-group.eu:443/backend/rs/shipments", this.providerSettings.ApiDomain)))
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

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var errorCode   = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("error"))?.Value?.ToString() ?? "Unknow";
                var message     = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("message"))?.Value?.ToString() ?? "Unknow";
                throw new ShipITException(errorCode, message);
            }
            
            List<RequestShippingLabelResponse> createdLabels = new List<RequestShippingLabelResponse>();

            if (response.Data == null) throw new ShipITException("No Data available in response", "NO_RESPONSE_DATA");

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

            #endregion

        }

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

        public async Task<CancelResult> CancelLabel(string cancelId, CancellationToken cancelToken = default)
        {

            var clientOptions = new RestClientOptions(new Uri(string.Format("https://shipit-wbm-{0}.gls-group.eu:443/backend/rs/shipments/cancel/{1}", this.providerSettings.ApiDomain, cancelId)))
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

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var errorCode = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("error"))?.Value?.ToString() ?? "Unknow";
                var message = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("message"))?.Value?.ToString() ?? "Unknow";
                throw new ShipITException(errorCode, message);
            }

            if(response.Data == null) throw new ShipITException("No Data available in response", "NO_RESPONSE_DATA");

            switch (response.Data.Result)
            {
                case "CANCELLED":
                case "CANCELLATION_PENDING":
                    return CancelResult.CANCLED;
                case "SCANNED":
                    return CancelResult.ALREADY_IN_USE;
                default:
                    throw new ShipITException(response.Data.Result, "UNKNOW_CANCEL_RESULT");
            }

        }
    }
}
