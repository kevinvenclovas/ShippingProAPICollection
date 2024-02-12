namespace ShippingProAPICollection.Provider.DHL.Entities
{
    public class ThreeLetterCountryCodeResolvingException : Exception
    {
        public ThreeLetterCountryCodeResolvingException(string message) : base(message) { }
    }
}
