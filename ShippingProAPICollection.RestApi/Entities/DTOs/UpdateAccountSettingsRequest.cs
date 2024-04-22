using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.RestApi.Entities.DTOs
{
    public class UpdateAccountSettingsRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        public required string Name { get; set; } = null!;
        /// <summary>
        /// Name2
        /// </summary>
        public string? Name2 { get; set; } = null!;
        /// <summary>
        /// Name3
        /// </summary>
        public string? Name3 { get; set; } = null!;
        /// <summary>
        /// Street
        /// </summary>
        public required string Street { get; set; } = null!;
        /// <summary>
        /// Post code
        /// </summary>
        public required string PostCode { get; set; } = null!;
        /// <summary>
        /// City
        /// </summary>
        public required string City { get; set; } = null!;
        /// <summary>
        /// Iso A2 country code - DE 
        /// </summary>
        public required string CountryIsoA2Code { get; set; } = null!;
        /// <summary>
        /// Contact name
        /// </summary>
        public required string ContactName { get; set; } = null!;
        /// <summary>
        /// E-Mail
        /// </summary>
        public required string Email { get; set; } = null!;
        /// <summary>
        /// Phone number
        /// </summary>
        public required string Phone { get; set; } = null!;
    }
}
