namespace ShippingProAPICollection.Models.Error
{
    public class ShippingProviderErrorCodeException : ShippingProviderException
    {
        public ShippingErrorCode ErrorCode { get; private set; }
        public object? Payload { get; private set; }

        public ShippingProviderErrorCodeException(ShippingErrorCode errorcode, string message, object? payload = null) : base(message)
        {
            ErrorCode = errorcode;
            Payload = payload;
        }
    }
}
