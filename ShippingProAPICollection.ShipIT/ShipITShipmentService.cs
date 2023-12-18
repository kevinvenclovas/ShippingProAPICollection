using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.ShipIT.Entities;
using ShippingProAPICollection.ShipIT.Entities.Create;
using ShippingProAPICollection.ShipIT.Entities.Create.Response;
using ShippingProAPICollection.ShipIT.Entities.Create.Services;


namespace ShippingProAPICollection.ShipIT
{
    public class ShipITShipmentService : IShippingProviderService<ShipITShipmentRequestModel>
    {
        private string apiContentType = "application/glsVersion1+json";
        private ShipITSettings providerSettings = null!;
        private ShippingProAPIAccountSettings accountSettings = null!;

        public ShipITShipmentService(ShippingProAPIAccountSettings accountSettings, ShipITSettings providerSettings)
        {
            this.accountSettings = accountSettings;
            this.providerSettings = providerSettings;
        }


        public async Task<List<ShippingLabelResponse>> CreateLabel(ShipITShipmentRequestModel request, CancellationToken cancelToken = default)
        {
            
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
                    ShipmentUnitReference = String.IsNullOrEmpty(request.ShipmentReference) ? null : new List<string>() { request.ShipmentReference }.ToArray(),
                });
            }

            // Create shipment
            Shipment shipment = new Shipment()
            {
                IncotermCode = request.IncotermCode?.ToString() ?? null,
                ShippingDate = request.ShippingTime.ToString("yyyy-MM-dd"),
                Product = request.ServiceProduct,
                Consignee = new Consignee()
                {
                    Address = new Address()
                    {
                        Name1 = request.Adressline1,
                        Name2 = request.Adressline2,
                        Name3 = request.Adressline3,
                        CountryCode = request.Country,
                        ZIPCode = request.ZIPCode,
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

            RestResponse<CreatedShipmentResponse> response = await client.ExecuteAsync<CreatedShipmentResponse>(clientRequest).ConfigureAwait(false);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var errorCode   = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("error"))?.Value?.ToString() ?? "Unknow";
                var message     = response.Headers?.ToList().FirstOrDefault(x => x.Name != null && x.Name.Equals("message"))?.Value?.ToString() ?? "Unknow";
                throw new ShipITShipmentRequestException(errorCode, message);
            }
            
            List<ShippingLabelResponse> createdLabels = new List<ShippingLabelResponse>();

            if (response.Data != null)
            {
                string requestId = Guid.NewGuid().ToString();

                for (int i = 0; i < response.Data.CreatedShipment.ParcelData.Count(); i++)
                {
                    createdLabels.Add(new ShippingLabelResponse()
                    {
                        CancelId = response.Data.CreatedShipment.ParcelData[i].TrackID,
                        ParcelNumber = response.Data.CreatedShipment.ParcelData[i].ParcelNumber,
                        Label = response.Data.CreatedShipment.PrintData[i].Data,
                    });
                }
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



        public Task<bool> DeleteLabel(string cancelId)
        {
            throw new NotImplementedException();
        }
    }
}
