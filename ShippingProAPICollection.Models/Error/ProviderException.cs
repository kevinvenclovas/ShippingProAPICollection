namespace ShippingProAPICollection.Models.Error
{
    public class ProviderException : Exception
    {
        public ProviderException(string message) : base(message) { }
    }
}
