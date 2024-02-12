
namespace ShippingProAPICollection.Provider.ShipIT
{
    public class ShipITSettings : ProviderSettings
    {
        /// <summary>
        /// Api domain is the XXXXXXX part of your GLS-ShipIT api url provides by ur GLS contact => https://shipit-wbm-XXXXXXX.gls-group.eu:443/backend/rs
        /// </summary>
        public required string ApiDomain { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string ContactID { get; set; }
    }
}
