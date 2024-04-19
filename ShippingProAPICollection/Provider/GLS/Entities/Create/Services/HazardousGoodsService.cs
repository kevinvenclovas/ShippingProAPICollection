using ShippingProAPICollection.Provider.GLS.Entities.Create;
using ShippingProAPICollection.Provider.GLS.Entities.Create.Services;

namespace ShippingProAPICollection.Provider.GLS.Entities.Create.Services
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
