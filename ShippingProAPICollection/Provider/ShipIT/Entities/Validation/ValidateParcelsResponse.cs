
namespace ShippingProAPICollection.Provider.ShipIT.Entities.Validation
{
    internal class ValidateParcelsResponse
    {
        public bool Success { get; set; }
        public required ShipmentValidationResult ValidationResult { get; set; }
    }
}
