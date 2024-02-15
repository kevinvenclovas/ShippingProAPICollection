using ShippingProAPICollection.Models;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Provider.GLS.Entities.Validation;

namespace ShippingProAPICollection.Provider
{
    internal interface IShippingProviderService
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
        public Task<ShippingCancelResult> CancelLabel(string cancelId, CancellationToken cancelToken);

        /// <summary>
        /// Prüfe einen Versandlabel request |
        /// Validate a shippint label request
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Task<ValidationReponse> ValidateLabel(RequestShipmentBase request, CancellationToken cancelToken);

        /// <summary>
        /// Prüfe wieviel Tage bis zur Auflieferung benötigt werden
        /// This operation estimates the days needed to deliver a shipment unit from the given origin address to the given destination address.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public Task<uint> GetEstimatedDeliveryDays(RequestShipmentBase request, CancellationToken cancelToken);
    }
}
