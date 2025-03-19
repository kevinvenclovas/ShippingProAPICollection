using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Provider.DHL.Entities.VASService
{
    public class VASIdentCheckService : VASService
    {
        public required DateTimeOffset DateOfBirth { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required MinimumAge MinimumAge { get; set; }

        [SetsRequiredMembers]

        public VASIdentCheckService(DateTimeOffset dateOfBirth, string firstName, string lastName, MinimumAge minimumAge) : base()
        {
            MinimumAge = minimumAge;
            DateOfBirth = dateOfBirth;
            FirstName = firstName;
            LastName = lastName;
        }

        public override void Validate()
        {
            base.Validate();
            if (!FirstName.RangeLenghtValidation(1, 35)) throw new ShipmentRequestNoValidStringLengthException("FirstName", 1, 35);
            if (!LastName.RangeLenghtValidation(1, 35)) throw new ShipmentRequestNoValidStringLengthException("LastName", 1, 35);
        }
    }

    public enum MinimumAge
    {
        A16,
        A18
    }

}
