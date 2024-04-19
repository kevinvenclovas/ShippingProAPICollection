using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.RestApi.Entities.DTOs
{
    public class UpdateAccountSettingsRequest
    {
        [Required]
        public required string Name { get; set; }
        public string? Name2 { get; set; }
        public string? Name3 { get; set; }
        [Required]
        public required string Street { get; set; }
        [Required]
        public required string PostCode { get; set; }
        [Required]
        public required string City { get; set; }
        [Required]
        public required string CountryIsoA2Code { get; set; }
        [Required]
        public required string ContactName { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Phone { get; set; }
    }
}
