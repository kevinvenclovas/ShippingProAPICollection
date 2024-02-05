using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.ShipIT;

namespace ShippingProAPICollection
{
    public class ShippingProAPICollectionService
    {

        Dictionary<string, IShippingProviderService> providerServices = new Dictionary<string, IShippingProviderService>();

        public ShippingProAPICollectionService(ShippingProAPIAccountSettings accountSettings, ShippingProAPICollectionSettings providerSettings)
        {
            Dictionary<string, ProviderSettings> providers = providerSettings.GetProviders();

            foreach (KeyValuePair<string, ProviderSettings> provider in providers)
            {
                providerServices.Add(provider.Key, BuildProviderService(accountSettings, provider.Value));
            }
        }

        private IShippingProviderService BuildProviderService(ShippingProAPIAccountSettings accountSettings, ProviderSettings settings)
        {
            switch (settings)
            {
                case ShipITSettings shipITSettings:
                    return new ShipITShipmentService(accountSettings, shipITSettings);
                default:  throw new Exception("provider not available");
            }
        }


        public async Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken ct = default)
        {
            request.Validate();

            if (providerServices.TryGetValue(request.Provider, out var service))
            {
                return await service.RequestLabel(request, ct);
            }

            throw new InvalidOperationException("Unknown shipping provider");
        }

        public async Task<CancelResult> CancelLabel(string provider, string cancelId, CancellationToken ct = default)
        {
            
            if (providerServices.TryGetValue(provider, out var service))
            {
                return await service.CancelLabel(cancelId, ct);
            }

            throw new InvalidOperationException("Unknown shipping provider");
        }
    }
}
