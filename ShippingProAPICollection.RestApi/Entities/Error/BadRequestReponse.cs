using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.RestApi.Entities.Error
{
    public class BadRequestReponse
    {
        public required string ErrorMessages { get; set; }

        [SetsRequiredMembers]
        public BadRequestReponse(string _errorMessages)
        {
            ErrorMessages = _errorMessages;
        }
    }
}
