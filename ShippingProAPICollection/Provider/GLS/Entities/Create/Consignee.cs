using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using ShippingProAPICollection.Provider.GLS.Entities;
using ShippingProAPICollection.Provider.GLS.Entities.Create;

namespace ShippingProAPICollection.Provider.GLS.Entities.Create
{
    internal class Consignee
    {
        [MaxLength(80)]
        public string? ConsigneeID { get; set; }
        [MaxLength(80)]
        public string? CostCenter { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public GLSConsigneeCategoryTypes Category { get; set; }
        [MaxLength(80)]
        public required Address Address { get; set; }
    }
}
