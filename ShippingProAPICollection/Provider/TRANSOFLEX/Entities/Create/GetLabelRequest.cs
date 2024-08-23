using Newtonsoft.Json;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create
{
    public class GetLabelRequest
    {
        [JsonProperty("aviso_shipment_id")]
        public required string AvisoShipmentId { get; set; }
        [JsonProperty("sessionToken")]
        public required string SessionToken { get; set; }
        [JsonProperty("parcel_id")]
        public required string ParcelId { get; set; }
        [JsonProperty("rotation")]
        public int Rotation { get; set; } = 90;
        [JsonProperty("format")]
        public string Format { get; set; } = "PDF";
        [JsonProperty("scale")]
        public int Scale { get; set; } = 100;
    }
}
