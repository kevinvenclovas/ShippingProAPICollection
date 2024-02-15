using ShippingProAPICollection.Provider.GLS.Entities.Create;

namespace ShippingProAPICollection.Provider.GLS.Entities.Create
{
    internal class ShipmentRequestData
    {
        public required Shipment Shipment { get; set; }
        public required PrintingOptions PrintingOptions { get; set; }
    }
}
