namespace ShippingProAPICollection.Provider.DPD
{
    public class DPDSettings : ProviderSettings
    {
        /// <summary>
        /// Api domain is the XXXXXXX part of your DPD api url => https://public-XXXXXXX.dpd.com/services/
        /// </summary>
        public required string ApiDomain { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string DepotNumber { get; set; }
        /// <summary>
        /// en_EN , de_DE
        /// </summary>
        public required string APILanguage { get; set; }
    }
}
