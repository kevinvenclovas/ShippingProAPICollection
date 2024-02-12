using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ShippingProAPICollection.Provider.ShipIT.Entities;

namespace ShippingProAPICollection.Provider.ShipIT.Entities.Create
{
    internal class ReturnLabels
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ShipITTemplateSet TemplateSet { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ShipITLabelDocFormat LabelFormat { get; set; }
    }
}
