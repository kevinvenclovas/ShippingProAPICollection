using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Models.Error
{
    public class ShipmentRequestNoValidStringFormatException : Exception
    {
        public required string Value { get; set; }

        [SetsRequiredMembers]
        public ShipmentRequestNoValidStringFormatException(string value) : base()
        {
            Value = value;
        }
    }
}
