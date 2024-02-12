using ShippingProAPICollection.Provider.ShipIT.Entities.Create.Services;

namespace ShippingProAPICollection.Provider.ShipIT.Entities.Create
{
    internal class ShipmentService
    {
        public Service Service { get; set; }
        public ShopReturnService ShopReturn { get; set; }
        public DepositService Deposit { get; set; }
        public HazardousGoodsService HazardousGood { get; set; }
    }
}
