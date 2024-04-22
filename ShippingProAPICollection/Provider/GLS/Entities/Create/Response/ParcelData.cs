namespace ShippingProAPICollection.Provider.GLS.Entities.Create.Response
{
    internal class ParcelData
    {
        public required string TrackID { get; set; }
        public required string ParcelNumber { get; set; }
        public required Barcodes Barcodes { get; set; }
    }
}
