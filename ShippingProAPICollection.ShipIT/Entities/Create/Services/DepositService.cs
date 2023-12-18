namespace ShippingProAPICollection.ShipIT.Entities.Create.Services
{
    internal class DepositService : Service
    {
        public override string ServiceName { get; } = "service_deposit";
        public string PlaceOfDeposit { get; set; }

        public DepositService()
        {
        }
    }
}
