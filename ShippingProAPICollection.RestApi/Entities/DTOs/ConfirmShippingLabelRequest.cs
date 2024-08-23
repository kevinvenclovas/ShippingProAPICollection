namespace ShippingProAPICollection.RestApi.Entities.DTOs
{
    public class ConfirmShippingLabelRequest
    {
        /// <summary>
        /// Contract id
        /// </summary>
        /// <example>123456789</example>
        public required string ContractID { get; set; }

        /// <summary>
        /// Confirm id of the shipping label
        /// </summary>
        /// <example>GLS</example>
        public required string ConfirmId { get; set; }
    }
}
