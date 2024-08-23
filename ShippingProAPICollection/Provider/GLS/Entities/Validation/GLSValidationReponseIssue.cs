using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.GLS.Entities.Validation
{
    public class GLSValidationReponseIssue
    {
        public required string Message { get; set; }
        public required ShippingErrorCode ErrorCode { get; set; }
    }
}
