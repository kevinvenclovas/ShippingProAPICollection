namespace ShippingProAPICollection.Models.Error
{
    public class ShipmentRequestException : Exception
    {
        public ShipmentRequestException(string message) : base(message) { }
    }
}
