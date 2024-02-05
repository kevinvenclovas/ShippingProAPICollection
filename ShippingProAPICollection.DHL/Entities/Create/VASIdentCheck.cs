using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities.Create
{
    public class VASIdentCheck
    {
        [Newtonsoft.Json.JsonProperty("firstName", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(35, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Newtonsoft.Json.JsonProperty("lastName", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(35, MinimumLength = 1)]
        public string LastName { get; set; }

        /// <summary>
        /// date of birth, used in conjunction with minimumAge and shipping date. Format yyyy-mm-dd is used.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("dateOfBirth", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset DateOfBirth { get; set; }

        /// <summary>
        /// Checks if recipient will have reached specified age by shipping date.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("minimumAge", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"A16|A18")]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public VASIdentCheckMinimumAge MinimumAge { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}
