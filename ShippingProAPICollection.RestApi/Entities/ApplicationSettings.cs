using ShippingProAPICollection.Models;
using ShippingProAPICollection.Provider;

namespace ShippingProAPICollection.RestApi.Entities
{
    public class ApplicationSettings
    {
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

        public List<ProviderSettings> ProviderSettings = new List<ProviderSettings>();

    }
}
