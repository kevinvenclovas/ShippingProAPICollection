using System.Xml.Serialization;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create
{
    [XmlRoot("request")]
    public class ShipmentRequest
    {
        [XmlElement("sessionToken")]
        public required string SessionToken { get; set; }

        [XmlElement("aviso_shipment_id")]
        public string? AvisoShipmentId { get; set; }

        [XmlElement("createOrderRequest")]
        public required RequestData Value { get; set; }
    }

    public class RequestData
    {
        [XmlElement("shipment")]
        public required Shipment Shipment { get; set; }
    }
}
