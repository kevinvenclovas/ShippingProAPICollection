using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.Models.Error
{
    public class ShippingProviderException : Exception
    {
        public ShippingErrorCode ErrorCode { get; private set; }
        public object? Payload { get; private set; }

        public ShippingProviderException(ShippingErrorCode errorcode, string message, object? payload = null) : base(message)
        {
            ErrorCode = errorcode;
            Payload = payload;
        }
    }
}
