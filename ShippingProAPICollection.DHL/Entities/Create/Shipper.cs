using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities.Create
{
    public class Shipper
    {
        /// <summary>
        /// Name1. Line 1 of name information
        /// </summary>
        [Newtonsoft.Json.JsonProperty("name1", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(50, MinimumLength = 1)]
        public string Name1 { get; set; }

        /// <summary>
        /// An optional, additional line of name information
        /// </summary>
        [Newtonsoft.Json.JsonProperty("name2", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(50, MinimumLength = 1)]
        public string Name2 { get; set; }

        /// <summary>
        /// An optional, additional line of name information
        /// </summary>
        [Newtonsoft.Json.JsonProperty("name3", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(50, MinimumLength = 1)]
        public string Name3 { get; set; }

        /// <summary>
        /// Line 1 of the street address. This is just the street name. Can also include house number.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("addressStreet", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(50, MinimumLength = 1)]
        public string AddressStreet { get; set; }

        /// <summary>
        /// Line 1 of the street address. This is just the house number. Can be added to street name instead.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("addressHouse", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(10, MinimumLength = 1)]
        public string AddressHouse { get; set; }

        /// <summary>
        /// Mandatory for all countries but Ireland that use a postal code system.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("postalCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(10, MinimumLength = 3)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[0-9A-Za-z]+([ -]?[0-9A-Za-z]+)*$")]
        public string PostalCode { get; set; }

        /// <summary>
        /// city
        /// </summary>
        [Newtonsoft.Json.JsonProperty("city", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(40, MinimumLength = 1)]
        public string City { get; set; }

        /// <summary>
        /// Shipper address country
        /// </summary>
        [Newtonsoft.Json.JsonProperty("country", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public Country Country { get; set; }

        /// <summary>
        /// optional contact name. (this is not the primary name printed on label)
        /// </summary>
        [Newtonsoft.Json.JsonProperty("contactName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(80, MinimumLength = 3)]
        public string ContactName { get; set; }

        /// <summary>
        /// Optional contact email address of the shipper
        /// </summary>
        [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(80, MinimumLength = 3)]
        public string Email { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}
