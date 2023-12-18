namespace ShippingProAPICollection.Models
{
    public class ShippingLabelResponse
    {
        /// <summary>
        /// Id um das Label zu Löschen |
        /// Id to cancel the label
        /// </summary>
        public required string CancelId { get; set; }

        /// <summary>
        /// Trackingnummer |
        /// Parcelnumber of the label
        /// </summary>
        public required string ParcelNumber { get; set; }

        /// <summary>
        /// Label als PDF in byte[] Format |
        /// Shippinglabel in byte[] format
        /// </summary> 
        public required byte[] Label { get; set; }
    }
}
