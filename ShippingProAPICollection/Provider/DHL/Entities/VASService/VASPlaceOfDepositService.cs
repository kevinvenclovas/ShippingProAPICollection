using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Provider.DHL.Entities.VASService
{
    public class VASPlaceOfDepositService : VASService
    {
        public required string PlaceOfDeposit { get; set; }

        [SetsRequiredMembers]
        public VASPlaceOfDepositService(string placeOfDeposit)
        {
            PlaceOfDeposit = placeOfDeposit;
        }

        public override void Validate()
        {
            base.Validate();
            if (!PlaceOfDeposit.RangeLenghtValidation(1, 60)) throw new ShipmentRequestNoValidStringLengthException("PlaceOfDeposit", 1, 60);
        }

    }
}
