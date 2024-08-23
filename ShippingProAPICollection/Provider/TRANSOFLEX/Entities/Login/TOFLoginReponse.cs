using Newtonsoft.Json;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Login
{
    public class TOFLoginReponse
    {
        [JsonProperty("Message")]
        public required string Message { get; set; }
        [JsonProperty("sessionToken")]
        public string? SessionToken { get; set; }
    }
}
