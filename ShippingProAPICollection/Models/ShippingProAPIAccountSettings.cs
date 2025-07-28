
namespace ShippingProAPICollection.Models
{
    public class ShippingProAPIAccountSettings
    {
        public required string Name { get; set; }
        public string? Name2 { get; set; }
        public string? Name3 { get; set; }
        public required string Street { get; set; }
        public required string PostCode { get; set; }
        public required string City { get; set; }
        public required string CountryIsoA2Code { get; set; }
        public required string ContactName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        private Dictionary<string, ShippingProAPIAccountSettings> AlternativeShippingFromAddress = new();

        public void AddAlternativeShippingFromAddress(string locationCode, ShippingProAPIAccountSettings shippingProAPIShipFromAddress)
        {
            if (AlternativeShippingFromAddress.ContainsKey(locationCode))
            {
                AlternativeShippingFromAddress[locationCode] = shippingProAPIShipFromAddress;
            }
            else
            {
                AlternativeShippingFromAddress.Add(locationCode, shippingProAPIShipFromAddress);
            }
        }

        public ShippingProAPIAccountSettings GetShipFromAddress(string locationCode)
        {
            if (AlternativeShippingFromAddress.TryGetValue(locationCode, out var address))
            {
                return address;
            }
            return this;
        }

    }
}
