using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.ShipIT.Entities
{
    public class ShipITException : ProviderException
    {
        public string ErrorCode { get; set; }
        
        public ShipITException(string message, string errorcode) : base(message)
        {
            ErrorCode = errorcode;
        }
    }
}
