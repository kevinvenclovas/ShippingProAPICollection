namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Validation
{
    public class ValidationMessageReponse
    {
        public required string Code { get; set; }
        public required string Level { get; set; }
        public required string Description { get; set; }
    }
}
