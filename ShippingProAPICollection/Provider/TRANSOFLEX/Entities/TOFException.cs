using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities
{
    public class TOFException : ShippingProviderException
    {
        public TOFException(ShippingErrorCode errorcode, string message, object? payload = null) : base(errorcode, message, payload)
        {
        }
    }
}
