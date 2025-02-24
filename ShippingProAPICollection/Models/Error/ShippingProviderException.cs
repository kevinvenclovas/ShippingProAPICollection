namespace ShippingProAPICollection.Models.Error
{
    public class ShippingProviderException : Exception
    {
        public ShippingProviderException(string? message) : base(message)
        {
           
        }
    }
}
