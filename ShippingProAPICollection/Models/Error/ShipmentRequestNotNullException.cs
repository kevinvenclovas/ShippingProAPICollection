using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Models.Error
{
    public class ShipmentRequestNotNullException : Exception
    {
        public required string Value { get; set; }

        [SetsRequiredMembers]
        public ShipmentRequestNotNullException(string value) : base()
        {
            Value = value;
        }
    }
}
