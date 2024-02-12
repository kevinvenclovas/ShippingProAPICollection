using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.Provider.DHL.Entities
{
    public class DHLPackingStationData
    {
        /// <summary>
        /// Postkundennummer |
        /// DHL customer id
        /// </summary>
        /// <example>1234567</example>
        [Required]
        public required string PostNumber { get; set; }

        /// <summary>
        /// Packstationnummer |
        /// number of the packing station
        /// </summary>
        /// <example>168</example>
        [Required]
        public required string PackstationNumber { get; set; }
        /// <summary>
        /// Postleitzahl |
        /// postcode of the packing station
        /// </summary>
        /// <example>73479</example>
        [Required]
        public required string ZIPCode { get; set; }
        /// <summary>
        /// Stadt |
        /// City of the packing station
        /// </summary>
        /// <example>Ellwangen</example>
        [Required]
        public required string City { get; set; }
    }
}
