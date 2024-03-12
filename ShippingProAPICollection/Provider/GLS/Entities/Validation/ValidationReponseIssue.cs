using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.GLS.Entities.Validation
{
    public class ValidationReponseIssue
    {
        public required string Message { get; set; }
        public required ShippingErrorCode ErrorCode { get; set; }
    }
}
