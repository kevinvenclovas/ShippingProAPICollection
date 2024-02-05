namespace ShippingProAPICollection.DHL.Entities.Create
{
    public class ShipmentOrderRequest
    {
        [Newtonsoft.Json.JsonProperty("profile", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [System.ComponentModel.DataAnnotations.StringLength(35)]
        public string Profile { get; set; }

        /// <summary>
        /// Shipment array having details for each shipment.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("shipments", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        [System.ComponentModel.DataAnnotations.MaxLength(30)]
        public System.Collections.Generic.ICollection<Shipment> Shipments { get; set; } = new System.Collections.ObjectModel.Collection<Shipment>();

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }
    }
}
