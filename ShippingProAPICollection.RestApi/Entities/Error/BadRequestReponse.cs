using ShippingProAPICollection.Models.Error;
using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.RestApi.Entities.Error
{

    public class BadRequestReponse
    {
        public required ShippingProviderException ProviderException { get; set; }

        [SetsRequiredMembers]
        public BadRequestReponse(ShippingProviderException _providerException)
        {
            ProviderException = _providerException;
        }

    }

}
