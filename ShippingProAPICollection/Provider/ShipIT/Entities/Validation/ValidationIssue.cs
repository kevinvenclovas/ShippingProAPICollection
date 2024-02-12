﻿
namespace ShippingProAPICollection.Provider.ShipIT.Entities.Validation
{
    internal class ValidationIssue
    {
        public required string Rule { get; set; }
        public required string Location { get; set; }
        public required string[] Parameters { get; set; }
    }
}