using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Provider.ShipIT.Entities.Validation;

namespace ShippingProAPICollection.Provider
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

        /// <summary>
        /// Prüfe einen Versandlabel request |
        /// Validate a shippint label request
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Task<ValidationReponse> ValidateLabel(RequestShipmentBase request, CancellationToken cancelToken);

    }
}
