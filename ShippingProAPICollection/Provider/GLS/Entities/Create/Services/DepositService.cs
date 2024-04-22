namespace ShippingProAPICollection.Provider.GLS.Entities.Create.Services
{
    internal class DepositService : Service
    {
        public override string ServiceName { get; } = "service_deposit";
        public required string PlaceOfDeposit { get; set; } = null!;

        public DepositService()
        {
        }
    }
}
