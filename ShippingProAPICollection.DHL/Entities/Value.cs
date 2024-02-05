using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities
{
    public class Value
    {
        /// <summary>
        /// iso 4217 3 character currency code accepted. Recommended to use EUR where possible
        /// </summary>
        [Newtonsoft.Json.JsonProperty("currency", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ValueCurrency Currency { get; set; }

        /// <summary>
        /// Numeric value
        /// </summary>
        [Newtonsoft.Json.JsonProperty("value", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Range(0D, 100000D)]
        public double Value1 { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}
