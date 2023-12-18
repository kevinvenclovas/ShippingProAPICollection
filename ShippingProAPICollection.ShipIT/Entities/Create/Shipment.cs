using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.ShipIT.Entities.Create
{
    internal class Shipment
    {
        public string[]? ShipmentReference { get; set; }
        [MaxLength(10)]
        public required string ShippingDate { get; set; }
        [MaxLength(2)]
        public string? IncotermCode { get; set; }
        [MaxLength(40)]
        public string? Identifier { get; set; }
        [MaxLength(40)]
        public string? Middleware { get; set; }
     
        [JsonConverter(typeof(StringEnumConverter))]
        public required ShipITProductType Product { get; set; }
        public required Consignee Consignee { get; set; }
        public required Shipper Shipper { get; set; }
        public required ShipmentUnit[] ShipmentUnit { get; set; }
        public ShipmentService[]? Service { get; set; }
    }
}
