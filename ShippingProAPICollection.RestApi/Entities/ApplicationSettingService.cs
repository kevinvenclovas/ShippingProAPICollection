using Newtonsoft.Json;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Provider;

namespace ShippingProAPICollection.RestApi.Entities
{
    public class ApplicationSettingService
    {
        private const string settingFilePath = "application-settings.json";
       
        public ApplicationSettingService()
        {
           
        }

        /// <summary>
        /// Update account settings
        /// </summary>
        /// <param name="collectionSettings"></param>
        /// <param name="newAccountSettings"></param>
        public void UpdateAccountSettings(ShippingProAPICollectionSettings collectionSettings, ShippingProAPIAccountSettings newAccountSettings)
        {
            // Load current settings
            var currentSettings = LoadApplicationSettings();
            // Update account settings
            currentSettings.AccountSettings = newAccountSettings;

            SaveAndUpdateCurrentSettings(collectionSettings, currentSettings);
        }

        /// <summary>
        /// Add or update provider settings
        /// </summary>
        /// <param name="collectionSettings"></param>
        /// <param name="newSettings"></param>
        public void AddOrUpdateProviderSettings(ShippingProAPICollectionSettings collectionSettings, ProviderSettings newSettings)
        {
            // Load current settings
            var currentSettings = LoadApplicationSettings();

            collectionSettings.AddSettings(newSettings.ContractID, newSettings);
            currentSettings.ProviderSettings = collectionSettings.GetProviders().Select(x => x.Value).ToList();
            SaveAndUpdateCurrentSettings(collectionSettings, currentSettings);
        }

        /// <summary>
        /// Delete provider by contract id
        /// </summary>
        /// <param name="collectionSettings"></param>
        /// <param name="contractID"></param>
        public void DeleteProvider(ShippingProAPICollectionSettings collectionSettings, string contractID)
        {
            // Load current settings
            var currentSettings = LoadApplicationSettings();
            var providerToDelete = currentSettings.ProviderSettings.FirstOrDefault(x => x.ContractID.Equals(contractID));
            if (providerToDelete != null)
            {
                currentSettings.ProviderSettings.Remove(providerToDelete);
                SaveAndUpdateCurrentSettings(collectionSettings, currentSettings);
            }
        }

        /// <summary>
        /// Build new collection settings  
        /// </summary>
        /// <returns></returns>
        public ShippingProAPICollectionSettings BuildCollectionSettings()
        {
            var currentSettings = LoadApplicationSettings();
            var s = new ShippingProAPICollectionSettings(currentSettings.AccountSettings);
            currentSettings.ProviderSettings.ForEach(x => s.AddSettings(x.ContractID, x));
            return s;
        }


        private void SaveAndUpdateCurrentSettings(ShippingProAPICollectionSettings collectionSettings, ApplicationSettings newSettings)
        {
            // Save settings to file
            SaveApplicationSettings(newSettings);
            // Update application settings
            collectionSettings.OverrideSettings(BuildCollectionSettings());
        }
        private void SaveApplicationSettings(ApplicationSettings newSettings)
        {
           File.WriteAllText(settingFilePath, JsonConvert.SerializeObject(newSettings));
        }

        /// <summary>
        /// Load current application settings from file or create new file
        /// </summary>
        /// <returns></returns>
        public ApplicationSettings LoadApplicationSettings()
        {
            if (!File.Exists(settingFilePath))
            {
                // Create json file if not exist
                SaveApplicationSettings(new ApplicationSettings());
            }

            string jsonText = File.ReadAllText(settingFilePath);
            return JsonConvert.DeserializeObject<ApplicationSettings>(jsonText) ?? new ApplicationSettings();
        }
    }
}
