using ShippingProAPICollection.Models.Entities;

namespace ShippingProAPICollection.Models
{
    public class RequestShippingLabelResponse
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

        /// <summary>
        /// Gewicht des Paketes in Kg
        /// Weight of the package Kg
        /// </summary>
        public required float Weight { get; set; }

        /// <summary>
        /// Art des Labels, Normaler versand, Express versand oder Retourenlabel
        /// Kind of the label, normal shipment, express shipment or shop return
        /// </summary>
        public required ShippingLabelType LabelType { get; set; }

        /// <summary>
        /// Weitere Werte über das Versandlabel
        /// Additional values about the shipping label
        /// </summary>
        public Dictionary<string, object>? AdditionalValues { get; set; }

    }
}
