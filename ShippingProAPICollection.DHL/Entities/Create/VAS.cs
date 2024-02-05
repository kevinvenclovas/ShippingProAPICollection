using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingProAPICollection.DHL.Entities.Create
{
    public partial class VAS
    {
        /// <summary>
        /// Preferred neighbour. Can be specified as text.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("preferredNeighbour", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public string PreferredNeighbour { get; set; }

        /// <summary>
        /// Preferred location. Can be specified as text.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("preferredLocation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public string PreferredLocation { get; set; }

        /// <summary>
        /// if used it will trigger checking the age of recipient
        /// </summary>
        [Newtonsoft.Json.JsonProperty("visualCheckOfAge", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"A16|A18")]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public VASVisualCheckOfAge VisualCheckOfAge { get; set; }

        /// <summary>
        /// Delivery can only be signed for by yourself personally.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("namedPersonOnly", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool NamedPersonOnly { get; set; }

        [Newtonsoft.Json.JsonProperty("identCheck", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public VASIdentCheck IdentCheck { get; set; }

        /// <summary>
        /// Delivery must be signed for by the recipient and not by DHL staff
        /// </summary>
        [Newtonsoft.Json.JsonProperty("signedForByRecipient", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool SignedForByRecipient { get; set; }

        /// <summary>
        /// Instructions and endorsement how to treat international undeliverable shipment. By default, shipments are returned if undeliverable. There are country specific rules whether the shipment is returned immediately or after a grace period.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("endorsement", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public VASEndorsement Endorsement { get; set; }

        /// <summary>
        /// Preferred day of delivery in format YYYY-MM-DD. Shipper can request a preferred day of delivery. The preferred day should be between 2 and 6 working days after handover to DHL.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("preferredDay", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [Newtonsoft.Json.JsonConverter(typeof(DateFormatConverter))]
        public DateTimeOffset PreferredDay { get; set; }

        /// <summary>
        /// Delivery can only be signed for by yourself personally or by members of your household.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("noNeighbourDelivery", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool NoNeighbourDelivery { get; set; }

        [Newtonsoft.Json.JsonProperty("additionalInsurance", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Value AdditionalInsurance { get; set; }

        /// <summary>
        /// Leaving this out is same as setting to false. Sperrgut.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("bulkyGoods", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool BulkyGoods { get; set; }

        [Newtonsoft.Json.JsonProperty("cashOnDelivery", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public VASCashOnDelivery CashOnDelivery { get; set; }

        /// <summary>
        /// Special instructions for delivery. 2 character code, possible values agreed in contract.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("individualSenderRequirement", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"[a-zA-Z0-9]{2}")]
        public string IndividualSenderRequirement { get; set; }

        /// <summary>
        /// Choice of premium vs economy parcel. Availability is country dependent and may be manipulated by DHL if choice is not available. Please review the label.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("premium", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool Premium { get; set; }

        /// <summary>
        /// Closest Droppoint Delivery to the droppoint closest to the address of the recipient of the shipment. For this kind of delivery either the phone number and/or the e-mail address of the receiver is mandatory. For shipments using DHL Paket International it is recommended that you choose one of the three delivery types: Economy, Premium, CDP. Otherwise, the current default for the receiver country will be picked.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("closestDropPoint", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool ClosestDropPoint { get; set; }

        /// <summary>
        /// Undeliverable domestic shipment can be forwarded and held at retail. Notification to email (fallback: consignee email) will be used.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("parcelOutletRouting", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ParcelOutletRouting { get; set; }

        [Newtonsoft.Json.JsonProperty("dhlRetoure", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public VASDhlRetoure DhlRetoure { get; set; }

        /// <summary>
        /// All import duties are paid by the shipper.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("postalDeliveryDutyPaid", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool PostalDeliveryDutyPaid { get; set; }

        private IDictionary<string, object> _additionalProperties;

        [Newtonsoft.Json.JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }
}
