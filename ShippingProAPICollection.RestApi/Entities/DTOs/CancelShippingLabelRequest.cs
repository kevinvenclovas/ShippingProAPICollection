namespace ShippingProAPICollection.RestApi.Entities.DTOs
{
    public class CancelShippingLabelRequest
    {
        /// <summary>
        /// Contract id
        /// </summary>
        /// <example>123456789</example>
        public required string ContractID { get; set; }

        /// <summary>
        /// Cancel id of the shipping label
        /// </summary>
        /// <example>GLS</example>
        public required string CancelId { get; set; }
    }
}
