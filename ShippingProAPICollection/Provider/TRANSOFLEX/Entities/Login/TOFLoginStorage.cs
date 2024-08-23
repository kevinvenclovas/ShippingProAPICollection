namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Login
{
    public class TOFLoginStorage
    {
        public required string ContractID { get; set; }
        public required string SessionToken { get; set; }
        public required DateTime ExpireTime { get; set; }
    }
}
