﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace ShippingProAPICollection.ShipIT.Entities.Create
{
    internal class Consignee
    {
        [MaxLength(80)]
        public string? ConsigneeID { get; set; }
        [MaxLength(80)]
        public string? CostCenter { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ShipITConsigneeCategoryTypes Category { get; set; }
        [MaxLength(80)]
        public required Address Address { get; set; }
    }
}
