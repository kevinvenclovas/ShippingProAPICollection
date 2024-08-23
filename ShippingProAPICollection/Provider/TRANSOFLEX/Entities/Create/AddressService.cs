using System.Xml.Serialization;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create
{
    [XmlRoot("services")]
    public class AddressService
    {
        [XmlElement("service")]
        public AddressServiceTypes Service { get; set; }
    }
}
