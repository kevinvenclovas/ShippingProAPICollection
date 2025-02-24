using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Models.Error
{
    public class ShipmentRequestLabelCountException : ShippingProviderException
    {
        public required uint Value { get; set; }

        [SetsRequiredMembers]
        public ShipmentRequestLabelCountException(uint value) : base(null)
        {
            Value = value;
        }
    }
}
