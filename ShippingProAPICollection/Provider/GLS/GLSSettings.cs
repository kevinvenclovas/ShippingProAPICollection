using ShippingProAPICollection.Models.Entities;

namespace ShippingProAPICollection.Provider.GLS
{
    public class GLSSettings : ProviderSettings
    {
        public override ShippingProviderType ShippingProviderType => ShippingProviderType.GLS;
        /// <summary>
        /// Api domain is the XXXXXXX part of your GLS-GLS api url provides by ur GLS contact => https://GLS-wbm-XXXXXXX.gls-group.eu:443/backend/rs
        /// </summary>
        public required string ApiDomain { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string ContactID { get; set; }
    }
}
