namespace ShippingProAPICollection.Provider.GLS.Entities.Create
{
    internal class HazardousGood
    {
        public required string GLSHazNo { get; set; }

        public required decimal Weight { get; set; }
    }
}
