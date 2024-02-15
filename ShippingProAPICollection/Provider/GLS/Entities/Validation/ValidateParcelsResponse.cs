
namespace ShippingProAPICollection.Provider.GLS.Entities.Validation
{
    internal class ValidateParcelsResponse
    {
        public bool Success { get; set; }
        public required ShipmentValidationResult ValidationResult { get; set; }
    }
}
