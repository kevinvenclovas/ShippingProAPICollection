using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Provider.DHL.Entities.VASService
{
    public class VASPreferredNeighbourService : VASService
    {
        public required string PreferredNeighbour {  get; set; }

        [SetsRequiredMembers]
        public VASPreferredNeighbourService(string preferredNeighbour)
        {
            PreferredNeighbour = preferredNeighbour;
        }

        public override void Validate()
        {
            base.Validate();
            if (!PreferredNeighbour.RangeLenghtValidation(0, 100)) throw new ShipmentRequestNoValidStringLengthException("PreferredNeighbour", null, 100);
        }

    }
}
