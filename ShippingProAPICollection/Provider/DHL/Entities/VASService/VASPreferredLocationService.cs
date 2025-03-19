using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Provider.DHL.Entities.VASService
{
    public class VASPreferredLocationService : VASService
    {
        public required string PreferredLocation {  get; set; }

        [SetsRequiredMembers]
        public VASPreferredLocationService(string preferredLocation)
        {
            PreferredLocation = preferredLocation;
        }

        public override void Validate()
        {
            base.Validate();
            if (!PreferredLocation.RangeLenghtValidation(0, 100)) throw new ShipmentRequestNoValidStringLengthException("PreferredLocation", null, 100);
        }

    }
}
