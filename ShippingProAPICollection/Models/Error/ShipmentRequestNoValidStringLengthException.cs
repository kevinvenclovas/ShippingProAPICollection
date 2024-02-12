﻿using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Models.Error
{
    public class ShipmentRequestNoValidStringLengthException : Exception
    {
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public required string Value { get; set; }

        [SetsRequiredMembers]
        public ShipmentRequestNoValidStringLengthException(string value, int? min, int max) : base()
        {
            Value = value;
            MinLength = min;
            MaxLength = max;
        }
    }
}
