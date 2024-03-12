
namespace ShippingProAPICollection.Provider.GLS.Entities.Create.Response
{
    internal class CreatedShipmentData
    {
        public List<string>? ShipmentReference { get; set; }
        public required List<ParcelData> ParcelData { get; set; }
        public required List<Document> PrintData { get; set; }
        public string? CustomerID { get; set; }
        public string? PickupLocation { get; set; }
    }
}
