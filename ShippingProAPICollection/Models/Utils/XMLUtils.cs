using System.Xml.Serialization;

namespace ShippingProAPICollection.Models.Utils
{
    public static class XMLUtils
    {
        public static string SerializeToXml<T>(this T value)
        {
            using (var stringwriter = new StringWriter())
            {
                var serializer = new XmlSerializer(value.GetType());
                serializer.Serialize(stringwriter, value);
                return stringwriter.ToString();
            }
        }

    }
}
