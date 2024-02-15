using ShippingProAPICollection.Provider.GLS.Entities.Create;

namespace ShippingProAPICollection.Provider.GLS.Entities.Validation
{
    internal class ValidateShipmentRequestData
    {
        public required Shipment Shipment { get; set; }
    }
}
