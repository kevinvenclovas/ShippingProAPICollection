using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities.Create
{
    public partial class BankAccount
    {
        [Newtonsoft.Json.JsonProperty("accountHolder", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.StringLength(80)]
        public string AccountHolder { get; set; }

        [Newtonsoft.Json.JsonProperty("bankName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(80)]
        public string BankName { get; set; }

        [Newtonsoft.Json.JsonProperty("iban", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"[A-Z]{2,2}[0-9]{2,2}[a-zA-Z0-9]{1,30}")]
        public string Iban { get; set; }

        [Newtonsoft.Json.JsonProperty("bic", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"[a-zA-Z0-9]{8,11}")]
        public string Bic { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}
