﻿namespace ShippingProAPICollection.ShipIT.Entities.Create.Response
{
    internal class Document
    {
        public required byte[] Data { get; set; }
        public string? LabelFormat { get; set; }
    }
}
