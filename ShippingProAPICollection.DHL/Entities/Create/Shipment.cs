namespace ShippingProAPICollection.DHL.Entities.Create
{
    public class Shipment
    {
        /// <summary>
        /// Determines the DHL Paket product to be used.
        /// <br/>
        /// <br/>* V01PAK: DHL PAKET;
        /// <br/>* V53WPAK: DHL PAKET International;
        /// <br/>* V54EPAK: DHL Europaket;
        /// <br/>* V62WP: Warenpost;
        /// <br/>* V66WPI: Warenpost International
        /// </summary>
        [Newtonsoft.Json.JsonProperty("product", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Product { get; set; }

        /// <summary>
        /// 14 digit long number that identifies the contract the shipment is booked on. Please note that in rare cases the last to characters can be letters. Digit 11 and digit 12 must correspondent to the number of the product, e.g. 333333333301tt can only be used for the product V01PAK (DHL Paket).
        /// </summary>
        [Newtonsoft.Json.JsonProperty("billingNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"\w{10}\d{2}\w{2}")]
        public string BillingNumber { get; set; }

        /// <summary>
        /// A reference number that the user can assign for better association purposes. It appears on shipment labels. To use the reference number for tracking purposes, it should be at least 8 characters long and unique.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("refNo", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(35, MinimumLength = 8)]
        public string RefNo { get; set; }

        /// <summary>
        /// Textfield that appears on the shipment label. It cannot be used to search for the shipment.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("costCenter", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CostCenter { get; set; }

        /// <summary>
        /// Is only to be indicated by DHL partners.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("creationSoftware", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CreationSoftware { get; set; }

        /// <summary>
        /// Date the shipment is transferred to DHL. The shipment date can be the current date or a date up to a few days in the future. It must not be in the past. Iso format required: yyyy-mm-dd. On the shipment date the shipment will be automatically closed at your end of day closing time.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("shipDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(DateFormatConverter))]
        public System.DateTimeOffset ShipDate { get; set; }

        /// <summary>
        /// Shipper information, including contact information and address. Alternatively, a predefined shipper reference can be used.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("shipper", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Shipper Shipper { get; set; }

        [Newtonsoft.Json.JsonProperty("consignee", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Consignee Consignee { get; set; }

        [Newtonsoft.Json.JsonProperty("details", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ShipmentDetails Details { get; set; }

        [Newtonsoft.Json.JsonProperty("services", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public VAS Services { get; set; }

        [Newtonsoft.Json.JsonProperty("customs", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public CustomsDetails Customs { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}
