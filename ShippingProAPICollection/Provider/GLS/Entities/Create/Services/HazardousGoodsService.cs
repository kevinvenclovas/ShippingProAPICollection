namespace ShippingProAPICollection.Provider.GLS.Entities.Create.Services
{
    internal class HazardousGoodsService : Service
    {
        public override string ServiceName { get; } = "service_hazardousgoods";
        public required HazardousGood[] HazardousGood { get; set; }

        public HazardousGoodsService() : base()
        {
        }
    }
}
