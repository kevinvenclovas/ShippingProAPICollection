using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RestSharp;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
using ShippingProAPICollection.Provider.TRANSOFLEX.Entities;
using ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Cancel;
using ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Confirm;
using ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create;
using ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create.Response;
using ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Login;
using ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Logout;

namespace ShippingProAPICollection.Provider.TRANSOFLEX
{

    public class TOFShipmentService : IShippingProviderService
    {
        private readonly IMemoryCache _cache;
        private TOFSettings providerSettings = null!;
        private ShippingProAPIAccountSettings accountSettings = null!;
        private Dictionary<string, TOFLoginStorage> loginStorage = new();
        private int sessionLifetime = 14;

        public TOFShipmentService(ShippingProAPIAccountSettings accountSettings, TOFSettings providerSettings, IMemoryCache cache)
        {
            this.accountSettings = accountSettings;
            this.providerSettings = providerSettings;
            this._cache = cache;
            LoadTOFLoginStorages();
        }

        public async Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken cancelToken = default)
        {
            var TOFRequest = request as TOFShipmentRequestModel;
            if (TOFRequest == null) throw new Exception("Cannot convert request to TOFShipmentRequestModel");

            var shipment = new Shipment()
            {
                CustomerNr = providerSettings.CustomerNr,
                ColliCount = request.Items.Count,
                PalettCount = 0,
                ShippingDate = DateTime.Now,
                Type = TOFRequest.ShipmentType,
                Weight = TOFRequest.Items.Sum(x => x.Weight),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        City = request.City,
                        CountryCode = request.Country,
                        Name1 = request.Adressline1,
                        Name2 = request.Adressline2,
                        Name3 = request.Adressline3,
                        PostCode = request.PostCode,
                        Street = request.Street,
                        StreetNumber = request.StreetNumber,
                        Type = AddressType.RECIPIENT,
                        Service = new AddressService()
                    }
                },
                References = new List<ShipmentReference>()
                {
                    new ShipmentReference()
                    {
                        Type = TOFShipmentReferenceTypes.SHIPMENT,
                        Value = TOFRequest.ShipmentReference
                    },
                    new ShipmentReference()
                    {
                        Type = TOFShipmentReferenceTypes.ALTERNATIVE,
                        Value = TOFRequest.Note1 ?? ""
                    },
                    new ShipmentReference()
                    {
                        Type = TOFShipmentReferenceTypes.ALTERNATIVE2,
                        Value = TOFRequest.Note2 ?? ""
                    }
                },
                Additionalinfos = new List<Info>()
                {
                    new Info()
                    {
                        Type = InfoType.OPERATIONAL,
                        Text = TOFRequest.ShipmentOperationInfo ?? ""
                    }
                },
                Packages = request.Items.Select(x => new Package() { Type = PackageType.C, Weight = x.Weight }).ToList()
            };

            ShipmentRequest shipmentRequest = new ShipmentRequest() { 
                SessionToken = await GetAuthToken(),
                Value = new RequestData() { Shipment = shipment } 
            };

            RestResponse<CreatedShipmentResponse> response = await CallApi<CreatedShipmentResponse>(
                new Uri(string.Format("{0}/order/v1/customer/shipment", providerSettings.ApiDomain)),
                Method.Post,
                shipmentRequest,
                BodyType.XML,
                cancelToken
                );

            HTTPReponseUtils.CheckHttpResponse<TOFException>("", shipmentRequest, response);

            List<RequestShippingLabelResponse> createdLabels = new List<RequestShippingLabelResponse>();

            if (response.Data == null)
            {
                throw new TOFException(ShippingErrorCode.UNKNOW, "No Data available in response", new { payload = shipmentRequest, response = response.Content });
            }
            else
            {
                for (int i = 0; i < response.Data.ParcelIds.Count(); i++)
                {
                    createdLabels.Add(new RequestShippingLabelResponse()
                    {
                        CancelId = response.Data.AvisoShipmentId,
                        ParcelNumber = response.Data.ParcelIds[i],
                        Label = await GetLabel(response.Data.AvisoShipmentId, response.Data.ParcelIds[i]),
                        LabelType = TOFRequest.ShipmentType == TOFShipmentType.PICKUP ? ShippingLabelType.SHOPRETURN : ShippingLabelType.NORMAL,
                        Weight = request.Items[i].Weight
                    });
                }
            }


            return createdLabels;

        }

        public async Task<ShippingCancelResult> CancelLabel(string cancelId, CancellationToken cancelToken = default)
        {
            CancelShipmentRequest cancelRequest = new CancelShipmentRequest()
            {
                SessionToken = await GetAuthToken(),
                AvisoShipmentId = cancelId
            };

            RestResponse<CreatedShipmentResponse> response = await CallApi<CreatedShipmentResponse>(
                new Uri(string.Format("{0}/order/v1/customer/shipment/delete", providerSettings.ApiDomain)),
                Method.Post,
                cancelRequest,
                BodyType.XML,
                cancelToken
                );

            HTTPReponseUtils.CheckHttpResponse<TOFException>(response.Content ?? "Unknow", cancelRequest, response);
            return ShippingCancelResult.CANCLED;
        }

        public async Task<ValidationReponse> ValidateLabel(RequestShipmentBase request, CancellationToken cancelToken)
        {
            throw new TOFException(ShippingErrorCode.NOT_AVAILABLE, "Feature not available for TOF");
        }

        public async Task<uint> GetEstimatedDeliveryDays(RequestShipmentBase request, CancellationToken cancelToken)
        {
            throw new TOFException(ShippingErrorCode.NOT_AVAILABLE, "Feature not available for TOF");
        }

        public async Task ConfirmShipment(string avisoShipmentId, CancellationToken cancelToken)
        {
            ConfirmShipmentRequest cancelRequest = new ConfirmShipmentRequest()
            {
                SessionToken = await GetAuthToken(),
                AvisoShipmentId = avisoShipmentId
            };

            RestResponse<CreatedShipmentResponse> response = await CallApi<CreatedShipmentResponse>(
                new Uri(string.Format("{0}/order/v1/customer/shipment/confirm", providerSettings.ApiDomain)),
                Method.Post,
                cancelRequest,
                BodyType.XML,
                cancelToken
                );

            HTTPReponseUtils.CheckHttpResponse<TOFException>(response.Content ?? "Unknow", cancelRequest, response);
        }

        /// <summary>
        /// Logout from TOF service
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="ShippingProviderErrorCodeException"></exception>
        private async Task Logout(string sessionToken, CancellationToken cancelToken = default)
        {
            var body = new TOFLogout() { SessionToken = sessionToken };

            RestResponse<TOFLogoutResponse> response = await CallApi<TOFLogoutResponse>(
                new Uri(string.Format("{0}/order/v1/user/logout", providerSettings.ApiDomain)),
                Method.Post,
                body,
                BodyType.XML,
                cancelToken
            );

            if (!(response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Unauthorized))
            {
                throw new ShippingProviderErrorCodeException(ShippingErrorCode.UNKNOW, response.ErrorMessage + "<------>" + response.Content, body);
            }

            loginStorage.Remove(providerSettings.ContractID);
            SaveTOFLoginStorages();
        }

        /// <summary>
        /// Login into TOF service
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        /// <exception cref="ShippingProviderErrorCodeException"></exception>
        private async Task<string> LoginToTOF(CancellationToken cancelToken = default)
        {
            string url = string.Format("{0}/order/v1/user/login", providerSettings.ApiDomain);

            var body = new Dictionary<string, string>
            {
                { "user", providerSettings.Username },
                { "password", providerSettings.Password },
            };

            RestResponse<TOFLoginReponse> response = await CallApi<TOFLoginReponse>(
                new Uri(string.Format("{0}/order/v1/user/login", providerSettings.ApiDomain)),
                Method.Post,
                body,
                BodyType.XWWWFORM,
                cancelToken
            );

            if (response.Data == null) throw new TOFException(ShippingErrorCode.UNKNOW, "No Data available in response", new { payload = body, response = response.Content });
            HTTPReponseUtils.CheckHttpResponse<TOFException>(response.Data.Message ?? "Unbekannter Fehler", body, response);

            if (
                String.IsNullOrEmpty(response.Data.Message) ||
                !(response.Data.Message.Equals("Success")) ||
                String.IsNullOrEmpty(response.Data.SessionToken)
            ) throw new TOFException(ShippingErrorCode.TOF_LOGIN_ERROR, "Login to TOF was not successful: " + response.Data.Message);

            AddSessionTokenToStorage(response.Data.SessionToken);

            return response.Data.SessionToken;
        }

        /// <summary>
        /// Call TOF API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="body"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        private async Task<RestResponse<T>> CallApi<T>(Uri url, Method method, object body, BodyType bodyType, CancellationToken cancelToken)
        {
            var clientOptions = new RestClientOptions(url)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            RestClient client = new RestClient(clientOptions);

            var clientRequest = new RestRequest()
            {
                Method = method
            };

            switch (bodyType)
            {
                case BodyType.JSON:

                    string json = JsonConvert.SerializeObject(body, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    });

                    clientRequest.AddJsonBody(json);
                    break;

                case BodyType.XML:

                    clientRequest.AddHeader("Content-Type", "application/xml");
                    var xml = body.SerializeToXml();
                    clientRequest.AddXmlBody(xml);
                    break;

                case BodyType.XWWWFORM:

                    clientRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                    Dictionary<string, string> bodyValues = body as Dictionary<string, string>;

                    foreach (var param in bodyValues)
                    {
                        clientRequest.AddParameter(param.Key, param.Value);
                    }
                    break;
            }

            var response = await client.ExecuteAsync<T>(clientRequest, cancelToken).ConfigureAwait(false);

            return response;

        }

        /// <summary>
        /// Get auth token from cache or login into TOF service
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TOFException"></exception>
        private async Task<string> GetAuthToken()
        {
            if (!loginStorage.ContainsKey(providerSettings.ContractID))
            {
                return await LoginToTOF();
            }
            
            var login = loginStorage[providerSettings.ContractID];
            if (login.ExpireTime < DateTime.Now)
            {
                await Logout(login.SessionToken);
                return await LoginToTOF();
            }

            return login.SessionToken;

        }

        /// <summary>
        /// Get top label
        /// </summary>
        /// <param name="avisoShipmentId"></param>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        /// <exception cref="TOFException"></exception>
        private async Task<byte[]> GetLabel(string avisoShipmentId, string parcelId)
        {
            GetLabelRequest request = new GetLabelRequest()
            { 
                SessionToken = await GetAuthToken(),
                AvisoShipmentId = avisoShipmentId,
                ParcelId = parcelId
            };

            var clientOptions = new RestClientOptions(new Uri(string.Format("{0}/order/v1/customer/label/get", providerSettings.ApiDomain)))
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            RestClient client = new RestClient(clientOptions);

            var clientRequest = new RestRequest()
            {
                Method = Method.Post
            };

            string json = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });

            clientRequest.AddHeader("Content-Type", "application/json");
            clientRequest.AddHeader("Accept", "application/pdf");
            clientRequest.AddJsonBody(json);

            var pdfBytes = await client.DownloadDataAsync(clientRequest);

            if (pdfBytes == null) throw new TOFException(ShippingErrorCode.TOF_DOWNLOAD_PDF_ERROR, "PDF konnte nicht gedownloaded werden", request);
            return pdfBytes;
        }


        #region Storage

        string tofStorageFileName = "TOFStorage.json";
        private void SaveTOFLoginStorages()
        {
            IEnumerable<TOFLoginStorage> storages = loginStorage.Select(x => x.Value);
            string json = JsonConvert.SerializeObject(storages);

            File.WriteAllText(tofStorageFileName, json);
        }

        private void LoadTOFLoginStorages()
        {
            if (File.Exists(tofStorageFileName))
            {
                string json = File.ReadAllText(tofStorageFileName);
                IEnumerable<TOFLoginStorage> storages = JsonConvert.DeserializeObject<IEnumerable<TOFLoginStorage>>(json) ?? [];
                loginStorage = storages.ToDictionary(x => x.ContractID, x => x);
            }
        }

        private void AddSessionTokenToStorage(string sessionToken)
        {
            // Add token to login storage
            if (loginStorage.ContainsKey(providerSettings.ContractID))
            {
                var storage = loginStorage[providerSettings.ContractID];
                storage.SessionToken = sessionToken;
                storage.ExpireTime = DateTime.Now.AddMinutes(sessionLifetime);
            }
            else
            {
                TOFLoginStorage storage = new TOFLoginStorage()
                {
                    ContractID = providerSettings.ContractID,
                    ExpireTime = DateTime.Now.AddMinutes(sessionLifetime),
                    SessionToken = sessionToken
                };

                loginStorage.Add(providerSettings.ContractID, storage);
            }
            SaveTOFLoginStorages();
        }
        
        #endregion

    }

    public enum BodyType
    {
        JSON,
        XML,
        XWWWFORM
    }
}
