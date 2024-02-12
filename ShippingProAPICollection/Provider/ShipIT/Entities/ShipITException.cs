﻿using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Provider.ShipIT.Entities
{
    public class ShipITException : ProviderException
    {
        public ShipITException(ErrorCode errorcode, string message, object? payload = null) : base(errorcode, message, payload)
        {
        }
    }
}