using ShippingProAPICollection.Provider.ShipIT.Entities.Create;

namespace ShippingProAPICollection.Provider.ShipIT.Entities.Validation
{
    internal class ValidateShipmentRequestData
    {
        public required Shipment Shipment { get; set; }
    }
}
