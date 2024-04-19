using ShippingProAPICollection.Provider.GLS.Entities.Create.Services;

namespace ShippingProAPICollection.Provider.GLS.Entities.Create.Services
{
    internal class ShopReturnService : Service
    {
        public override string ServiceName { get; } = "service_shopreturn";
        public long NumberOfLabels { get; set; }

        public ShopReturnService()
        {
        }
    }
}
