using System.ComponentModel.DataAnnotations;
using ShippingProAPICollection.Provider.ShipIT.Entities.Create;

namespace ShippingProAPICollection.Provider.ShipIT.Entities.Create
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
