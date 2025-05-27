
using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.Models
{
    public class ShippingProAPIShipFromAddress
    {
        public required string Name { get; set; }
        public string? Name2 { get; set; }
        public string? Name3 { get; set; }
        public required string Street { get; set; }
        public required string PostCode { get; set; }
        public required string City { get; set; }
        public required string CountryIsoA2Code { get; set; }
        public required string ContactName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
    }
}
