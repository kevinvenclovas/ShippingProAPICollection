using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.RestApi.Entities.Error
{
    public class InternalServerErrorReponse
    {
        public required string ErrorMessages { get; set; }

        [SetsRequiredMembers]
        public InternalServerErrorReponse(string _errorMessages)
        {
            ErrorMessages = _errorMessages;
        }
    }
}
