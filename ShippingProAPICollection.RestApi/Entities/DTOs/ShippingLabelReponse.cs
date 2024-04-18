namespace ShippingProAPICollection.RestApi.Entities.DTOs
{
    public class ShippingLabelReponse
    {
        /// <summary>
        /// Parcelnumber of the shipping label
        /// </summary>
        public required string ParcelNumber { get; set; }

        /// <summary>
        /// Label data
        /// </summary>
        public required byte[] Label { get; set; }

        /// <summary>
        /// Id to cancel shipping label
        /// </summary>
        public required string CancelId { get; set; }

        /// <summary>
        /// Tracking url
        /// </summary>
        public string? TrackingURL { get; set; }
    }
}
