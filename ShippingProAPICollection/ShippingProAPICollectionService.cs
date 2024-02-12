using Microsoft.Extensions.Caching.Memory;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Provider;
using ShippingProAPICollection.Provider.DHL;
using ShippingProAPICollection.Provider.DPD;
using ShippingProAPICollection.Provider.ShipIT;
using ShippingProAPICollection.Provider.ShipIT.Entities.Validation;

namespace ShippingProAPICollection
{
    public class ShippingProAPICollectionService
    {

        Dictionary<string, IShippingProviderService> providerServices = new Dictionary<string, IShippingProviderService>();

        public ShippingProAPICollectionService(IMemoryCache _cache, ShippingProAPIAccountSettings accountSettings, ShippingProAPICollectionSettings providerSettings)
        {
            Dictionary<string, ProviderSettings> providers = providerSettings.GetProviders();

            foreach (KeyValuePair<string, ProviderSettings> provider in providers)
            {
                providerServices.Add(provider.Key, BuildProviderService(_cache, accountSettings, provider.Value));
            }
        }

        private IShippingProviderService BuildProviderService(IMemoryCache _cache, ShippingProAPIAccountSettings accountSettings, ProviderSettings settings)
        {
            switch (settings)
            {
                case ShipITSettings providerSettings:
                    return new ShipITShipmentService(accountSettings, providerSettings);
                case DHLSettings providerSettings:
                    return new DHLShipmentService(accountSettings, providerSettings);
                case DPDSettings providerSettings:
                    return new DPDShipmentService(accountSettings, providerSettings, _cache);
                default:  throw new Exception("provider not available");
            }
        }

        public async Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken ct = default)
        {
            request.Validate();

            if (providerServices.TryGetValue(request.ContractID, out var service))
            {
                return await service.RequestLabel(request, ct);
            }

            throw new InvalidOperationException("Unknown shipping provider");
        }

        public async Task<ValidationReponse> ValidateLabel(RequestShipmentBase request, CancellationToken ct = default)
        {
            request.Validate();

            if (providerServices.TryGetValue(request.ContractID, out var service))
            {
                return await service.ValidateLabel(request, ct);
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
