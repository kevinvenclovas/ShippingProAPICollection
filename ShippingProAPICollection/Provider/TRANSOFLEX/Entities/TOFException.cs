using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities
{
    public class TOFException : ShippingProviderErrorCodeException
    {
        public TOFException(ShippingErrorCode errorcode, string message, object? payload = null) : base(errorcode, message, payload)
        {
        }
    }
}
