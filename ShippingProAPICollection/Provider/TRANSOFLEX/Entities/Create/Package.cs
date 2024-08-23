using System.Globalization;
using System.Xml.Serialization;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create
{
    public class Package
    {
        [XmlElement("type")]
        public required PackageType Type { get; set; }

        [XmlIgnore]
        public required float Weight { get; set; }

        [XmlElement("weight")]
        public string WeightFormatted
        {
            get => Weight.ToString("F2", CultureInfo.GetCultureInfo("de-DE"));
            set => Weight = float.Parse(value, CultureInfo.GetCultureInfo("de-DE"));
        }
    }
}
