﻿using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.Provider.DHL.Entities
{
    public class DHLPostOfficeData
    {
        /// <summary>
        /// Postfilalennummer |
        /// Number of the post office
        /// </summary>
        /// <example>110</example>
        public required string PostfilialeNumber { get; set; }
        /// <summary>
        /// Postkundennummer |
        /// DHL customer id
        /// </summary>
        /// <example>1234567</example>
        public required string PostNumber { get; set; }
        /// <summary>
        /// Postleitzahl |
        /// Postcode of the post office
        /// </summary>
        /// <example>73479</example>
        public required string PostCode { get; set; }
        /// <summary>
        /// Stadt |
        /// City of the post office
        /// </summary>
        /// <example>Ellwangen</example>
        public required string City { get; set; }
    }
}
