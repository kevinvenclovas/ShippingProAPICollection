using ShippingProAPICollection.Provider.GLS.Entities.Create.Services;

namespace ShippingProAPICollection.Provider.GLS.Entities.Create
{
    internal class ShipmentService
    {
        public Service Service { get; set; }
        public ShopReturnService ShopReturn { get; set; }
        public DepositService Deposit { get; set; }
        public HazardousGoodsService HazardousGood { get; set; }
    }
}
