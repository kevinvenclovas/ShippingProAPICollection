using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.DHL.Entities
{
    public class DHLException : ShippingProviderException
    {
        public DHLException(ShippingErrorCode errorcode, string message, object? payload = null) : base(errorcode, message, payload)
        {
        }
    }
}
