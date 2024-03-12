namespace ShippingProAPICollection.Provider.GLS.Entities.Cancel
{
    internal class CancelShipmentResponse
    {
        public required string TrackID { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "result")]
        public required string Result { get; set; }
    }
}
