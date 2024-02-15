using Microsoft.Extensions.Caching.Memory;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Provider;
using ShippingProAPICollection.Provider.DHL;
using ShippingProAPICollection.Provider.DPD;
using ShippingProAPICollection.Provider.GLS;
using ShippingProAPICollection.Provider.GLS.Entities.Validation;

namespace ShippingProAPICollection
{
    public class ShippingProAPICollectionService
    {

        Dictionary<string, IShippingProviderService> providerServices = new Dictionary<string, IShippingProviderService>();

        public ShippingProAPICollectionService(IMemoryCache _cache, ShippingProAPICollectionSettings providerSettings)
        {
            Dictionary<string, ProviderSettings> providers = providerSettings.GetProviders();

            foreach (KeyValuePair<string, ProviderSettings> provider in providers)
            {
                providerServices.Add(provider.Key, BuildProviderService(_cache, providerSettings.AccountSettings, provider.Value));
            }
        }

        private IShippingProviderService BuildProviderService(IMemoryCache _cache, ShippingProAPIAccountSettings accountSettings, ProviderSettings settings)
        {
            switch (settings)
            {
                case GLSSettings providerSettings:
                    return new GLSShipmentService(accountSettings, providerSettings);
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

        public async Task<uint> GetEstimatedDeliveryDays(RequestShipmentBase request, CancellationToken ct = default)
        {
            request.Validate();

            if (providerServices.TryGetValue(request.ContractID, out var service))
            {
                return await service.GetEstimatedDeliveryDays(request, ct);
            }

            throw new InvalidOperationException("Unknown shipping provider");
        }

        public async Task<ShippingCancelResult> CancelLabel(string contractID, string cancelId, CancellationToken ct = default)
        {
            
            if (providerServices.TryGetValue(contractID, out var service))
            {
                return await service.CancelLabel(cancelId, ct);
            }

            throw new InvalidOperationException("Unknown shipping provider");
        }
    
        public void ResetDPDAutToken()
        {
            foreach (var item in providerServices.Where(x => x.Value.GetType() == typeof(DPDShipmentService)).Select(x => x.Value as DPDShipmentService))
            {
                item.ResetDPDAutToken();
            }
        }

    }
}
