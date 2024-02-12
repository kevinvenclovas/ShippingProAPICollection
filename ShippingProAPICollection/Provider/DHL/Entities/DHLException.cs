using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.DHL.Entities
{
    public class DHLException : ProviderException
    {
        public DHLException(ErrorCode errorcode, string message, object? payload = null) : base(errorcode, message, payload)
        {
        }
    }
}
