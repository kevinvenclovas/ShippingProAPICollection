using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.Provider.ShipIT.Entities.Create
{
    internal class Address
    {
        [MaxLength(40)]
        public required string Name1 { get; set; }
        [MaxLength(40)]
        public string? Name2 { get; set; }
        [MaxLength(40)]
        public string? Name3 { get; set; }
        [MaxLength(2)]
        public required string CountryCode { get; set; }
        [MaxLength(40)]
        public string? Province { get; set; }
        [MaxLength(10)]
        public required string ZIPCode { get; set; }
        [MaxLength(40)]
        public required string City { get; set; }
        [MaxLength(40)]
        public required string Street { get; set; }
        [MaxLength(40)]
        public required string StreetNumber { get; set; }
        [MaxLength(80)]
        [Newtonsoft.Json.JsonProperty(PropertyName = "eMail")]
        public string? EMail { get; set; }
        [MaxLength(40)]
        public string? ContactPerson { get; set; }
        [MaxLength(35)]
        public string? FixedLinePhonenumber { get; set; }
        [MaxLength(35)]
        public string? MobilePhoneNumber { get; set; }
    }
}
