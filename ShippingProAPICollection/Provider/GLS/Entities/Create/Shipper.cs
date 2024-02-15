using System.ComponentModel.DataAnnotations;
using ShippingProAPICollection.Provider.GLS.Entities.Create;

namespace ShippingProAPICollection.Provider.GLS.Entities.Create
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
