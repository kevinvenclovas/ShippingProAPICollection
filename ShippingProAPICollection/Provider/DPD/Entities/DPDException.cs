using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.DPD.Entities
{
    public class DPDException : ShippingProviderException
    {
        public DPDException(ShippingErrorCode errorcode, string message, object? payload = null) : base(errorcode, message, payload)
        {
        }
    }
}
