using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create
{
    [XmlRoot("info")]
    public class Info
    {
        [XmlElement("type")]
        public required InfoType Type { get; set; }

        [XmlElement("text")]
        [MaxLength(60)]
        public required string Text { get; set; }
    }
}
