using System.Xml.Serialization;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Logout
{
    [Serializable]
    [XmlRoot("request")]
    public class TOFLogout
    {
        [XmlElement("sessionToken")]
        public required string SessionToken { get; set; }
    }

}
