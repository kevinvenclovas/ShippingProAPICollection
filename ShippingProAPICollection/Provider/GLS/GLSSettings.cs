using ShippingProAPICollection.Models.Entities;

namespace ShippingProAPICollection.Provider.GLS
{
    public class GLSSettings : ProviderSettings
    {
        public override ShippingProviderType ShippingProviderType => ShippingProviderType.GLS;
        /// <summary>
        /// Api domain => https://shipit-wbm-test01.gls-group.eu:443
        /// </summary>
        public required string ApiDomain { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string ContactID { get; set; }
    }
}
