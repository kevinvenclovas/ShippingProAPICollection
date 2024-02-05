using ShippingProAPICollection.Models;

namespace ShippingProAPICollection.DHL
{
    public class DHLSettings : ProviderSettings
    {
        public required string NationalAccountNumber { get; set; }
        public required string InternationalAccountNumber { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string ClientUsername { get; set; }
        public required string ClientPassword { get; set; }
    }
}
