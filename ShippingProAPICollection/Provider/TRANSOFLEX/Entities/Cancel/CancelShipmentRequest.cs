using System.Xml.Serialization;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Cancel
{
    [XmlRoot("request")]
    public class CancelShipmentRequest
    {
        [XmlElement("sessionToken")]
        public required string SessionToken { get; set; }

        [XmlElement("aviso_shipment_id")]
        public string? AvisoShipmentId { get; set; }
    }
}
