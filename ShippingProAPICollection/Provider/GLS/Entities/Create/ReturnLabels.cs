using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ShippingProAPICollection.Provider.GLS.Entities;

namespace ShippingProAPICollection.Provider.GLS.Entities.Create
{
    internal class ReturnLabels
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public GLSTemplateSet TemplateSet { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public GLSLabelDocFormat LabelFormat { get; set; }
    }
}
