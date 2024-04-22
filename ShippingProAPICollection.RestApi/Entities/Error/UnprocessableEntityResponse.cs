using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.RestApi.Entities.Error
{
    public class UnprocessableEntityResponse
    {
        public required IEnumerable<string> ErrorMessages { get; set; }

        [SetsRequiredMembers]
        public UnprocessableEntityResponse(IEnumerable<string> _errorMessages)
        {
            ErrorMessages = _errorMessages;
        }
    }
}
