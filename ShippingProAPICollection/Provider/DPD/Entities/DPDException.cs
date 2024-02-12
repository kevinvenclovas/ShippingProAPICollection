using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.DPD.Entities
{
    public class DPDException : ProviderException
    {
        public DPDException(ErrorCode errorcode, string message, object? payload = null) : base(errorcode, message, payload)
        {
        }
    }
}
