namespace ShippingProAPICollection.ShipIT.Entities.Create.Response
{
    internal class ParcelData
    {
        public string TrackID { get; set; }
        public string ParcelNumber { get; set; }
        public Barcodes Barcodes { get; set; }
    }
}
