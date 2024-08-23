using System.Xml.Serialization;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Confirm
{

    [XmlRoot("request")]
    public class ConfirmShipmentRequest
    {
        [XmlElement("sessionToken")]
        public required string SessionToken { get; set; }

        [XmlElement("aviso_shipment_id")]
        public string? AvisoShipmentId { get; set; }
    }
}
