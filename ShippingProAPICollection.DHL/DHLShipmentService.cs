using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using System.Net.Http.Headers;

namespace ShippingProAPICollection.DHL
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

        public async Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken cancelToken = default)
        {

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic REPLACE_BASIC_AUTH");
          
            DHLClient client = new DHLClient();





        }

        public async Task<CancelResult> CancelLabel(string cancelId, CancellationToken cancelToken = default)
        {

        }
    }
}
