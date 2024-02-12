using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Models.Error
{
    public class ShipmentRequestLabelCountException : Exception
    {
        public required uint Value { get; set; }

        [SetsRequiredMembers]
        public ShipmentRequestLabelCountException(uint value)
        {
            Value = value;
        }
    }
}
