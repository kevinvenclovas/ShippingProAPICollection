namespace ShippingProAPICollection.Models.Error
{
    public class ProviderException : Exception
    {
        public ErrorCode ErrorCode { get; private set; }
        public object? Payload { get; private set; }

        public ProviderException(ErrorCode errorcode, string message, object? payload = null) : base(message)
        {
            ErrorCode = errorcode;
            Payload = payload;
        }
    }
}
