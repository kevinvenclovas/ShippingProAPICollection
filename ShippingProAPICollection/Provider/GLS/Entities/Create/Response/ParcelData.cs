using ShippingProAPICollection.Provider.GLS.Entities.Create.Response;

namespace ShippingProAPICollection.Provider.GLS.Entities.Create.Response
{
    internal class ParcelData
    {
        public string TrackID { get; set; }
        public string ParcelNumber { get; set; }
        public Barcodes Barcodes { get; set; }
    }
}
