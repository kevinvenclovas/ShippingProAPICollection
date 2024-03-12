
namespace ShippingProAPICollection.Provider.GLS.Entities.Validation
{
    internal class ValidationIssue
    {
        public required string Rule { get; set; }
        public required string Location { get; set; }
        public string[]? Parameters { get; set; }
    }
}
