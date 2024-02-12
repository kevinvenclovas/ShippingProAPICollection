using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.ShipIT.Entities.Validation
{
    public class ValidationReponseIssue
    {
        public required string Message { get; set; }
        public required ErrorCode ErrorCode { get; set; }
    }
}
