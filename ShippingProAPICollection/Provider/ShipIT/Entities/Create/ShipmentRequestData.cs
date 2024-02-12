using ShippingProAPICollection.Provider.ShipIT.Entities.Create;

namespace ShippingProAPICollection.Provider.ShipIT.Entities.Create
{
    internal class ShipmentRequestData
    {
        public required Shipment Shipment { get; set; }
        public required PrintingOptions PrintingOptions { get; set; }
    }
}
