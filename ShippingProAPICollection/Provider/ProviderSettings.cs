using JsonSubTypes;
using Newtonsoft.Json;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Provider.DHL;
using ShippingProAPICollection.Provider.DPD;
using ShippingProAPICollection.Provider.GLS;
using ShippingProAPICollection.Provider.TRANSOFLEX;

namespace ShippingProAPICollection.Provider
{
    [JsonConverter(typeof(JsonSubtypes), nameof(ShippingProviderType))]
    [JsonSubtypes.KnownSubType(typeof(GLSSettings), ShippingProviderType.GLS)]
    [JsonSubtypes.KnownSubType(typeof(DPDSettings), ShippingProviderType.DPD)]
    [JsonSubtypes.KnownSubType(typeof(DHLSettings), ShippingProviderType.DHL)]
    [JsonSubtypes.KnownSubType(typeof(TOFSettings), ShippingProviderType.TRANSOFLEX)]
    public abstract class ProviderSettings
    {
        /// <summary>
        /// Provider type
        /// </summary>
        public abstract ShippingProviderType ShippingProviderType { get; }
        /// <summary>
        /// Contract id
        /// </summary>
        public string ContractID { get; set; } = null!;

        public abstract float MaxPackageWeight { get; set; }
    }
}
