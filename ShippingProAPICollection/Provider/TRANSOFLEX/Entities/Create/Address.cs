using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create
{
    public class Address
    {
        [XmlElement("type")]
        public required AddressType Type { get; set; }

        [XmlElement("name1")]
        public required string Name1 { get; set; }

        [XmlElement("name2")]
        [MaxLength(40)]
        public string? Name2 { get; set; }

        [XmlElement("name3")]
        [MaxLength(40)]
        public string? Name3 { get; set; }

        [XmlElement("street")]
        public required string Street { get; set; }

        [XmlElement("number")]
        public string? StreetNumber { get; set; }

        [XmlElement("country")]
        [MaxLength(2)]
        public required string CountryCode { get; set; }

        [XmlElement("code")]
        [MaxLength(10)]
        public required string PostCode { get; set; }

        [XmlElement("city")]
        [MaxLength(40)]
        public required string City { get; set; }

        [XmlElement("services")]
        public required AddressService Service { get; set; }
    }
}
