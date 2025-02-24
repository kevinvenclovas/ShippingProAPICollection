using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Models.Error
{
    public class ShipmentRequestWeightException : ShippingProviderException
    {
        public required float MinWeight { get; set; }
        public required float MaxWeight { get; set; }
        public required float CurrentWeight { get; set; }

        [SetsRequiredMembers]
        public ShipmentRequestWeightException(float minWeight, float maxWeight, float currentWeight) : base(null)
        {
            MinWeight = minWeight;
            MaxWeight = maxWeight;
            CurrentWeight = currentWeight;
        }
    }
}
