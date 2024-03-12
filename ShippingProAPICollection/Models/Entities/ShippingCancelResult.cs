﻿namespace ShippingProAPICollection.Models.Entities
{
    public enum ShippingCancelResult
    {

        /// <summary>
        /// Label was successfully canceled
        /// </summary>
        CANCLED,

        /// <summary>
        /// Label already scanned or in use, cannot cancel label anymore
        /// </summary>
        ALREADY_IN_USE
    }
}
