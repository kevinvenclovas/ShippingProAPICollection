using ShippingProAPICollection.Models.Error;
using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.RestApi.Entities.Error
{
    public class InternalServerErrorReponse
    {
        public required ShippingProviderException ProviderException { get; set; }

        [SetsRequiredMembers]
        public InternalServerErrorReponse(ShippingProviderException _providerException)
        {
            ProviderException = _providerException;
        }

    }
}
