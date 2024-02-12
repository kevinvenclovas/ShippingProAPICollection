using ShippingProAPICollection.Provider.ShipIT.Entities.Create;
using ShippingProAPICollection.Provider.ShipIT.Entities.Create.Services;

namespace ShippingProAPICollection.Provider.ShipIT.Entities.Create.Services
{
    internal class HazardousGoodsService : Service
    {
        public override string ServiceName { get; } = "service_hazardousgoods";
        public HazardousGood[] HazardousGood { get; set; }

        public HazardousGoodsService()
        {
        }
    }
}
