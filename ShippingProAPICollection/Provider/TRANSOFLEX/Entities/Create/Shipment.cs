using System.Globalization;
using System.Xml.Serialization;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create
{
    public class Shipment
    {
        [XmlElement("customer")]
        public required string CustomerNr { get; set; }

        [XmlIgnore]
        public required DateTime ShippingDate { get; set; }

        [XmlElement("date")]
        public string ShipmentDateFormatted
        {
            get { return ShippingDate.ToString("dd.MM.yyyy"); }
            set { ShippingDate = DateTime.Parse(value); }
        }

        [XmlElement("palletcount")]
        public required int PalettCount { get; set; }

        [XmlElement("collicount")]
        public required int ColliCount { get; set; }

        [XmlIgnore]
       
        public required float Weight { get; set; }

        [XmlElement("weight")]
        public string WeightFormatted
        {
            get => Weight.ToString("F2", CultureInfo.GetCultureInfo("de-DE"));
            set => Weight = float.Parse(value, CultureInfo.GetCultureInfo("de-DE"));
        }

        [XmlElement("type")]
        public required TOFShipmentType Type { get; set; }

        [XmlArray("references")]
        [XmlArrayItem("reference")]
        public List<ShipmentReference> References { get; set; }

        [XmlArray("addresses")]
        [XmlArrayItem("address")]
        public List<Address> Addresses { get; set; }

        [XmlArray("additionalinfos")]
        [XmlArrayItem("info")]
        public List<Info>? Additionalinfos { get; set; }

        [XmlArray("packages")]
        [XmlArrayItem("package")]
        public required List<Package> Packages { get; set; }

    }
}
