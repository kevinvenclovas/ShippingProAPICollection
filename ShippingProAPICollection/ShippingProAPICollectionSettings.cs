using ShippingProAPICollection.Models;
using ShippingProAPICollection.ShipIT;

namespace ShippingProAPICollection
{
    public class ShippingProAPICollectionSettings
    {
        private List<ProviderSettings> liveSettings { get; set; } = new List<ProviderSettings>();
        private List<ProviderSettings> devSettings { get; set; } = new List<ProviderSettings>();

        public ShippingProAPICollectionSettings()
        {
        }

        public void InitializeShipIT(ShipITSettings liveSettings, ShipITSettings? devSettings)
        {
            AddSettings(liveSettings);
            if (devSettings != null) AddDevSettings(devSettings);
        }





        public T GetLiveSettings<T>() where T : class
        {
            var settings = liveSettings.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
            if (settings == null) 
                throw new Exception($"{typeof(T).Name} Provider not initalized");
            return settings;
        }

        public T GetDevSettings<T>() where T : class
        {
            var settings = devSettings.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
            if (settings == null)
                throw new Exception($"{typeof(T).Name} Provider not initalized");
            return settings;
        }


        private void AddSettings(ProviderSettings setting)
        {
            if (liveSettings.Any(x => x.GetType() == setting.GetType())) throw new Exception($"{ setting.GetType().Name} Provider already initalized");
            liveSettings.Add(setting);
        }

        private void AddDevSettings(ProviderSettings setting)
        {
            if (devSettings.Any(x => x.GetType() == setting.GetType())) throw new Exception($"{setting.GetType().Name} Provider already initalized");
            devSettings.Add(setting);
        }
    }
}
