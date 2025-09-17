using ShippingProAPICollection.Models.Entities;

namespace ShippingProAPICollection.Provider.DHL
{
    public class DHLSettings : ProviderSettings
    {
        public override ShippingProviderType ShippingProviderType => ShippingProviderType.DHL;
        /// <summary>
        /// Api domain is the XXXXXXX part of the api url provides by DHL => https://api-XXXXXXX.dhl.com/parcel/de/shipping/v2/	
        /// </summary>
        public required string ApiDomain { get; set; }
        public required string NationalAccountNumber { get; set; }
        public required string InternationalAccountNumber { get; set; }
        public string WarenpostNationalAccountNumber { get; set; } = string.Empty;
        public string WarenpostInternationalAccountNumber { get; set; } = string.Empty;
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string LabelPrintFormat { get; set; }
        public required string DHLShipmentProfile { get; set; }
        public required string APIKey { get; set; }
        /// <summary>
        /// en-US or de-DE
        /// </summary>
        public required string APILanguage { get; set; }
        public override float MaxPackageWeight { get; set; } = 31.5f;
    }
}
