namespace ShippingProAPICollection.Provider.DHL.Entities
{
    public enum DHLProductType
    {
        /// <summary>
        /// Normalversand |
        /// Normal shipping
        /// </summary>
        V01PAK,
        /// <summary>
        /// Expressversand |
        /// Express shipping
        /// </summary>
        V01PRIO,
        /// <summary>
        /// Normalversand-International |
        /// Normal international shipping
        /// </summary>
        V53WPAK,
        /// <summary>
        /// Kleinpaket (max. 35 x 25 x 8 cm) |
        /// DHL small package (max. 35 x 25 x 8 cm)
        /// </summary>
        V62KP,
    }
}
