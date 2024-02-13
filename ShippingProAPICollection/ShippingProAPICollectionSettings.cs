using ShippingProAPICollection.Models;
using ShippingProAPICollection.Provider;
using ShippingProAPICollection.Provider.DHL;
using ShippingProAPICollection.Provider.DPD;
using ShippingProAPICollection.Provider.ShipIT;

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
            if (providerSettings.ContainsKey(contractID)) throw new Exception($"{contractID} Provider already initalized");
            setting.ContractID = contractID;
            providerSettings.Add(contractID, setting);
        }

        public void AddSettings(ProviderSettings setting)
        {
            string contractID = null!;

            switch (setting)
            {
                case ShipITSettings:
                    contractID = "GLS";
                    break;
                case DHLSettings:
                    contractID = "DHL";
                    break;
                case DPDSettings:
                    contractID = "DPD";
                    break;
                default: throw new Exception("provider not available");
            }

            if (providerSettings.ContainsKey(contractID)) throw new Exception($"{contractID} Provider already initalized");
            setting.ContractID = contractID;
            providerSettings.Add(contractID, setting);
        }

    }
}
