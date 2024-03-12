namespace ShippingProAPICollection.Provider.DHL.Entities
{
    public enum DHLServiceType
    {
        /// <summary>
        /// 
        /// </summary>
        NONE,
        /// <summary>
        /// Deposit: Paket an einem Wunschort hinterlegen |
        /// Deposit: Deposit the package at a desired location
        /// </summary>
        DEPOSIT,
        /// <summary>
        /// Lieferung an Packstation |
        /// Delivery to packing station
        /// </summary>
        LOCKER,
        /// <summary>
        /// Lieferung an Postfiliale |
        /// Delivery to post office
        /// </summary>
        POSTOFFICE,
    }
}
