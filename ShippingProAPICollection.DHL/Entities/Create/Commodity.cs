using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities.Create
{
    public class Commodity
    {
        /// <summary>
        /// A text that describes the commodity item. Only the first 50 characters of the description text is printed on the customs declaration form CN23.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("itemDescription", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(256, MinimumLength = 1)]
        public string ItemDescription { get; set; }

        /// <summary>
        /// A valid country code consisting of three characters according to ISO 3166-1 alpha-3.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("countryOfOrigin", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Country CountryOfOrigin { get; set; }

        /// <summary>
        /// Harmonized System Code aka Customs tariff number.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("hsCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(11, MinimumLength = 6)]
        public string HsCode { get; set; }

        /// <summary>
        /// How many items of that type are in the package
        /// </summary>
        [Newtonsoft.Json.JsonProperty("packagedQuantity", Required = Newtonsoft.Json.Required.Always)]
        public int PackagedQuantity { get; set; }

        [Newtonsoft.Json.JsonProperty("itemValue", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Value ItemValue { get; set; } = new Value();

        [Newtonsoft.Json.JsonProperty("itemWeight", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Weight ItemWeight { get; set; } = new Weight();

        private IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}
