using ShippingProAPICollection.Models;
using ShippingProAPICollection.Provider;

namespace ShippingProAPICollection
{
    public class ShippingProAPICollectionSettings
    {
        public ShippingProAPIAccountSettings AccountSettings { get; private set; }
        private Dictionary<string, ProviderSettings> providerSettings { get; set; } = new Dictionary<string, ProviderSettings>();

        public ShippingProAPICollectionSettings(ShippingProAPIAccountSettings accountSettings)
        {
            this.AccountSettings = accountSettings;
        }

        public Dictionary<string, ProviderSettings> GetProviders()
        {
            return providerSettings;
        }

        public void AddSettings(string contractID, ProviderSettings setting)
        {
            if (providerSettings.ContainsKey(contractID))
            {
                providerSettings.Remove(contractID);
            }

            setting.ContractID = contractID;
            providerSettings.Add(contractID, setting);
        }

        public void OverrideSettings(ShippingProAPICollectionSettings newSettings)
        {
            AccountSettings = newSettings.AccountSettings;
            providerSettings = newSettings.GetProviders();
        }
    }
}
