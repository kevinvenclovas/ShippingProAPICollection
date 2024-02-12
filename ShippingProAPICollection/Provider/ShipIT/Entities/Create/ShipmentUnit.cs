using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.Provider.ShipIT.Entities.Create
{
    internal class ShipmentUnit
    {
        public string[]? ShipmentUnitReference { get; set; }
        public decimal Weight { get; set; }
        [MaxLength(50)]
        public string? Note1 { get; set; }
        [MaxLength(50)]
        public string? Note2 { get; set; }
        [StringLength(18, MinimumLength = 18)]
        public string? FRAlphaParcelReference { get; set; }
        [MaxLength(40)]
        public string? TrackID { get; set; }
        public string? ParcelNumber { get; set; }
    }
}
