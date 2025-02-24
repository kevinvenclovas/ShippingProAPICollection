using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.GLS.Entities
{
    public class GLSException : ShippingProviderErrorCodeException
    {
        public GLSException(ShippingErrorCode errorcode, string message, object? payload = null) : base(errorcode, message, payload)
        {
        }
    }
}
