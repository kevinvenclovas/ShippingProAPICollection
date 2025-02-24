using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Models.Error
{
    public class ShipmentRequestNotNullException : ShippingProviderException
    {
        public required string Value { get; set; }

        [SetsRequiredMembers]
        public ShipmentRequestNotNullException(string value) : base(null)
        {
            Value = value;
        }
    }
}
