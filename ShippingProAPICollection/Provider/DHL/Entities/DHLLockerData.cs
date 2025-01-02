using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.Provider.DHL.Entities
{
    public class DHLLockerData
    {
        /// <summary>
        /// Postkundennummer |
        /// DHL customer id
        /// </summary>
        /// <example>1234567</example>
        public required string PostNumber { get; set; }

        /// <summary>
        /// Packstationnummer |
        /// number of the packing station
        /// </summary>
        /// <example>168</example>
        public required string PackstationNumber { get; set; }
        /// <summary>
        /// Postleitzahl |
        /// postcode of the packing station
        /// </summary>
        /// <example>73479</example>
        public required string PostCode { get; set; }
        /// <summary>
        /// Stadt |
        /// City of the packing station
        /// </summary>
        /// <example>Ellwangen</example>
        public required string City { get; set; }
    }
}
