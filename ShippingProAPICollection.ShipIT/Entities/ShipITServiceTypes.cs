namespace ShippingProAPICollection.ShipIT.Entities
{
    public enum ShipITServiceType
    {
        /// <summary>
        /// </summary>
        NONE,
        /// <summary>
        /// Deposit: Paket an einem Ort hinterlegen |
        /// Deposit: Deposit the package at a desired location
        /// </summary>
        DEPOSIT,
        /// <summary>
        /// G24 Service, Lieferung in den nächsten 24Stunden |
        /// G24, delivery in the next 14 hours
        /// </summary>
        G24,
        /// <summary> 
        /// GLS Express Termin 8Uhr - Lieferung bis morgen 8Uhr |
        /// Delivery till 8 o'clock next day
        /// </summary>
        G8,
        /// <summary>
        /// GLS Express Termin 9Uhr - Lieferung bis morgen 9Uhr |
        /// Delivery till 9 o'clock next day
        /// </summary>
        G9,
        /// <summary>
        /// GLS Express Termin 10Uhr - Lieferung bis morgen 10Uhr |
        /// Delivery till 10 o'clock next day
        /// </summary>
        G10,
        /// <summary>
        /// GLS Express Termin 12Uhr - Lieferung bis morgen 12Uhr |
        /// Delivery till 12 o'clock next day
        /// </summary>
        G12,
        /// <summary>
        /// GLS Express Termin Samstag 10Uhr - Lieferung bis morgen 10Uhr |
        /// Saturday delivery till 10 o'clock next day
        /// </summary>
        GSATURDAY10,
        /// <summary>
        /// GLS Express Termin Samstag 12Uhr - Lieferung bis morgen 12Uhr |
        /// Saturday delivery till 12 o'clock next day
        /// </summary>
        GSATURDAY12,
        /// <summary>
        /// GLS Rücksendung |
        /// Return
        /// </summary>
        SHOPRETURN
    }
}
