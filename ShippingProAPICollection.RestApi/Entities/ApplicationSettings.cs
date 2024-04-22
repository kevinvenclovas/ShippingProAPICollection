using ShippingProAPICollection.Models;
using ShippingProAPICollection.Provider;

namespace ShippingProAPICollection.RestApi.Entities
{
    /// <summary>
    /// Shipping pro api collection settings
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// Account settings
        /// </summary>
        public ShippingProAPIAccountSettings AccountSettings = 
            new ShippingProAPIAccountSettings() 
            { 
                City = "", 
                ContactName = "", 
                CountryIsoA2Code = "", 
                Email = "", 
                Name = "", 
                Phone = "",
                PostCode = "", 
                Street = ""
            };

        /// <summary>
        /// Provider settings
        /// </summary>
        public List<ProviderSettings> ProviderSettings = new List<ProviderSettings>();

    }
}
