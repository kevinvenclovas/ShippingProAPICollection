using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create
{
    public class ShipmentReference
    { 
        [XmlElement("type")]
        public required TOFShipmentReferenceTypes Type { get; set; }

        [XmlElement("value")]
        [MaxLength(15)]
        public required string Value { get; set; }
    }

}
