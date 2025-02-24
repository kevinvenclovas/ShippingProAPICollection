using ShippingProAPICollection.Models.Error;
using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.RestApi.Entities.Error
{

    public class BadRequestReponse
    {
        public required ShippingProviderErrorCodeException ProviderException { get; set; }

        [SetsRequiredMembers]
        public BadRequestReponse(ShippingProviderErrorCodeException _providerException)
        {
            ProviderException = _providerException;
        }

    }

}
