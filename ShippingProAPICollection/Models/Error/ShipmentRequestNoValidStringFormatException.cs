using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Models.Error
{
    public class ShipmentRequestNoValidStringFormatException : ShippingProviderException
    {
        public required string Value { get; set; }

        [SetsRequiredMembers]
        public ShipmentRequestNoValidStringFormatException(string value) : base(null)
        {
            Value = value;
        }
    }
}
