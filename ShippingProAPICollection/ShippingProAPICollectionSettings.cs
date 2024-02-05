using ShippingProAPICollection.Models;

namespace ShippingProAPICollection
{
    public class ShippingProAPICollectionSettings
    {
        private Dictionary<string, ProviderSettings> providerSettings { get; set; } = new Dictionary<string, ProviderSettings>();

        public ShippingProAPICollectionSettings()
        {
        }

        public Dictionary<string, ProviderSettings> GetProviders()
        {
            return providerSettings;
        }

        public void AddSettings(string providerKey, ProviderSettings setting)
        {
            if (providerSettings.ContainsKey(providerKey)) throw new Exception($"{providerKey} Provider already initalized");
            providerSettings.Add(providerKey,setting);
        }

    }
}
