using ShippingProAPICollection.Models.Entities;

namespace ShippingProAPICollection.Models
{
    public interface IShippingProviderService
    {
        /// <summary>
        /// Erstellen ein Versandlabels |
        /// Create shipping label
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public Task<List<RequestShippingLabelResponse>> RequestLabel(RequestShipmentBase request, CancellationToken cancelToken);

        /// <summary>
        /// Stornieren eines Versandlabels mit der Stornierungsnummer | 
        /// Cancel shipping label
        /// </summary>
        /// <param name="cancelId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public Task<CancelResult> CancelLabel(string cancelId, CancellationToken cancelToken);

    }
}
