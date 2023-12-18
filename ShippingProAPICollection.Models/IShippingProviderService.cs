namespace ShippingProAPICollection.Models
{
    public interface IShippingProviderService<T>
    {
        /// <summary>
        /// Erstellen ein Versandlabels |
        /// Create shipping label
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="request"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public Task<List<ShippingLabelResponse>> CreateLabel(T request, CancellationToken cancelToken);

        /// <summary>
        /// Stornieren eines Versandlabels mit der Stornierungsnummer | 
        /// Cancel shipping label
        /// </summary>
        /// <param name="cancelId"></param>
        /// <returns></returns>
        public Task<bool> DeleteLabel(string cancelId);
    }
}
