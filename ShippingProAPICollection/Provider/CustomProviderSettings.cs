using Microsoft.Extensions.Caching.Memory;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;

namespace ShippingProAPICollection.Provider
{
    public abstract class CustomProviderSettings : ProviderSettings
    {
        public override ShippingProviderType ShippingProviderType => ShippingProviderType.CUSTOM;
        public abstract IShippingProviderService CreateProviderService(ShippingProAPIAccountSettings accountSettings, IMemoryCache _cache);
    }
}
