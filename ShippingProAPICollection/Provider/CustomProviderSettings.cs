using Microsoft.Extensions.Caching.Memory;
using ShippingProAPICollection.Models;

namespace ShippingProAPICollection.Provider
{
    public abstract class CustomProviderSettings : ProviderSettings
    {
        public abstract IShippingProviderService CreateProviderService(ShippingProAPIAccountSettings accountSettings, IMemoryCache _cache);
    }
}
