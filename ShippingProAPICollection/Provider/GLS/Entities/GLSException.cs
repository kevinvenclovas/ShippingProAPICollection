using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.GLS.Entities
{
    public class GLSException : ShippingProviderException
    {
        public GLSException(ShippingErrorCode errorcode, string message, object? payload = null) : base(errorcode, message, payload)
        {
        }
    }
}
