using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities.Create
{
    public class CustomsDetails
    {
        /// <summary>
        /// Invoice number
        /// </summary>
        [Newtonsoft.Json.JsonProperty("invoiceNo", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(35)]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// This contains the category of goods contained in parcel.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("exportType", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CustomsDetailsExportType ExportType { get; set; }

        /// <summary>
        /// Mandatory if exporttype is 'OTHER'
        /// </summary>
        [Newtonsoft.Json.JsonProperty("exportDescription", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(80)]
        public string ExportDescription { get; set; }

        /// <summary>
        /// Aka 'Terms of Trade' aka 'Frankatur'. The attribute is exclusively used for the product Europaket (V54EPAK). DDU is deprecated (use DAP instead).
        /// </summary>
        [Newtonsoft.Json.JsonProperty("shippingConditions", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CustomsDetailsShippingConditions ShippingConditions { get; set; }

        /// <summary>
        /// Permit number. Very rarely needed. Mostly relevant for higher value goods. An example use case would be an item made from crocodile leather which requires dedicated license / permit identified by that number.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("permitNo", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public string PermitNo { get; set; }

        /// <summary>
        /// Attest or certification identified by this number. Very rarely needed. An example use case would be a medical shipment referring to an attestation that a certain amount of medicine may be imported within e.g. the current quarter of the year.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("attestationNo", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public string AttestationNo { get; set; }

        /// <summary>
        /// flag confirming whether electronic record for export was made
        /// </summary>
        [Newtonsoft.Json.JsonProperty("hasElectronicExportNotification", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool HasElectronicExportNotification { get; set; }

        [Newtonsoft.Json.JsonProperty("MRN", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(18)]
        public string MRN { get; set; }

        [Newtonsoft.Json.JsonProperty("postalCharges", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public Value PostalCharges { get; set; } = new Value();

        /// <summary>
        /// Optional. Will appear on CN23.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("officeOfOrigin", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(35)]
        public string OfficeOfOrigin { get; set; }

        /// <summary>
        /// Optional. The customs reference is used by customs authorities to identify economics operators an/or other persons involved. With the given reference, granted authorizations and/or relevant processes in customs clearance an/or taxation can be taken into account. Aka Zoll-Nummer or EORI-Number but dependent on destination.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("shipperCustomsRef", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(35)]
        public string ShipperCustomsRef { get; set; }

        /// <summary>
        /// Optional. The customs reference is used by customs authorities to identify economics operators an/or other persons involved. With the given reference, granted authorizations and/or relevant processes in customs clearance an/or taxation can be taken into account. Aka Zoll-Nummer or EORI-Number but dependent on destination.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("consigneeCustomsRef", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(35)]
        public string ConsigneeCustomsRef { get; set; }

        /// <summary>
        /// Commodity types in that package
        /// </summary>
        [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        [System.ComponentModel.DataAnnotations.MaxLength(99)]
        public System.Collections.Generic.ICollection<Commodity> Items { get; set; } = new System.Collections.ObjectModel.Collection<Commodity>();

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}
