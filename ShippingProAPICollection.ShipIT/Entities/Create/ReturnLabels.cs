using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShippingProAPICollection.ShipIT.Entities.Create
{
    internal class ReturnLabels
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ShipITTemplateSet TemplateSet { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ShipITLabelFormat LabelFormat { get; set; }
    }
}
