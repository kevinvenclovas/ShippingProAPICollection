using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.ShipIT.Entities
{
    public class ShipITShipmentRequestException : ShipmentRequestException
    {
        public string ErrorCode { get; set; }
        
        public ShipITShipmentRequestException(string message, string errorcode) : base(message)
        {
            ErrorCode = errorcode;
        }
    }
}
