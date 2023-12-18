using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.ShipIT.Entities.Create
{
    internal class Shipper
    {
        [MaxLength(20)]
        public required string ContactID { get; set; }
        public Address? AlternativeShipperAddress { get; set; }
        [StringLength(10, MinimumLength = 10)]
        public string? FRAlphaCustomerReference { get; set; }
    }
}
