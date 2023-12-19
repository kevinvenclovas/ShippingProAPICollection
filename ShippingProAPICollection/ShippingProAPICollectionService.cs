using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.ShipIT;

namespace ShippingProAPICollection
{
    public class ShippingProAPICollectionService
    {

        Dictionary<ProviderType, IShippingProviderService> providerServices;

        public ShippingProAPICollectionService(ShippingProAPIAccountSettings accountSettings, ShipITSettings providerSettings)
        {
            providerServices = new Dictionary<ProviderType, IShippingProviderService>()
            {
                 { ProviderType.SHIPIT , new ShipITShipmentService( accountSettings, providerSettings ) },
            };
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

        public async Task<CancelResult> CancelLabel(ProviderType provider, string cancelId, CancellationToken ct = default)
        {
            
            if (providerServices.TryGetValue(provider, out var service))
            {
                return await service.CancelLabel(cancelId, ct);
            }

            throw new InvalidOperationException("Unknown shipping provider");
        }
    }
}
