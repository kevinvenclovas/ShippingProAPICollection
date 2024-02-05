using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities.Create
{
    public partial class VASCashOnDelivery
    {
        [Newtonsoft.Json.JsonProperty("amount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Value Amount { get; set; }

        [Newtonsoft.Json.JsonProperty("bankAccount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BankAccount BankAccount { get; set; }

        /// <summary>
        /// Reference to bank account details. Account references are maintained in customer settings in Post &amp; DHL business customer portal under Ship -&gt; Settings -&gt; Cash on delivery. Please note, that the default account reference is used if the provided account reference does not exist in your customer settings!
        /// </summary>
        [Newtonsoft.Json.JsonProperty("accountReference", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(35)]
        public string AccountReference { get; set; }

        [Newtonsoft.Json.JsonProperty("transferNote1", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(35)]
        public string TransferNote1 { get; set; }

        [Newtonsoft.Json.JsonProperty("transferNote2", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(35)]
        public string TransferNote2 { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}
