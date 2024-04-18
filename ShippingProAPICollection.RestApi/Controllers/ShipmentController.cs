using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Provider;
using ShippingProAPICollection.Provider.GLS.Entities.Validation;
using ShippingProAPICollection.RestApi.Entities;
using ShippingProAPICollection.RestApi.Entities.DTOs;
using ShippingProAPICollection.RestApi.Helper;

namespace ShippingProAPICollection.RestApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController(
        ShippingProAPICollectionService shippingProAPICollectionService, 
        ShippingProAPICollectionSettings shippingProAPICollectionSettings,
        ApplicationSettingService applicationSettingService
        ) : ControllerBase
    {

        /// <summary>
        /// Anfragen eines neuen Labels
        /// Request new shipping labels
        /// </summary>
        [HttpPost]
        [Route("label")]
        [EnableCors("Intern")]
        [ProducesResponseType(typeof(List<ShippingLabelReponse>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> RequestShippingLabel([FromBody, BindRequired] RequestShipmentBase request)
        {
            var requestedLabels = await shippingProAPICollectionService.RequestLabel(request);

            List<ShippingLabelReponse> resultLabels = requestedLabels.Select(x => new ShippingLabelReponse()
            {
                CancelId = x.CancelId,
                Label = x.Label,
                ParcelNumber = x.ParcelNumber,
                TrackingURL = TrackingLinkHelper.CreateTrackingURL(x, request)
            }).ToList();
            
            return Ok(requestedLabels);
        }

        /// <summary>
        /// Anfragen eines neuen Labels
        /// Request new shipping labels
        /// </summary>
        [HttpDelete]
        [Route("label")]
        [EnableCors("Intern")]
        [ProducesResponseType(typeof(CancelShippingLabelReponse), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> CancelShippingLabel([FromBody, BindRequired] CancelShippingLabelRequest request)
        {
            var cancelResult = await shippingProAPICollectionService.CancelLabel(request.ContractID, request.CancelId);
            return Ok(new CancelShippingLabelReponse(cancelResult));
        }

        /// <summary>
        /// Anfragen eines neuen Labels
        /// Request new shipping labels
        /// </summary>
        [HttpPost]
        [Route("label/validate")]
        [EnableCors("Intern")]
        [ProducesResponseType(typeof(ValidationReponse), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> ValidateShippingLabel([FromBody, BindRequired] RequestShipmentBase request)
        {
            var validationRespone = await shippingProAPICollectionService.ValidateLabel(request);
            return Ok(validationRespone);
        }

        /// <summary>
        /// Löschen eines Providers
        /// Delete shipping provider
        /// </summary>
        [HttpGet]
        [Route("settings")]
        [EnableCors("Intern")]
        [ProducesResponseType(typeof(ApplicationSettings), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> GetApplicationSettings()
        {
            return Ok(applicationSettingService.LoadApplicationSettings());
        }

        /// <summary>
        /// Updaten von Providereinstellungen
        /// Update provider settings
        /// </summary>
        [HttpPut]
        [Route("settings/provider")]
        [EnableCors("Intern")]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> AddOrUpdateProvider([FromBody, BindRequired] ProviderSettings dto)
        {
            applicationSettingService.AddOrUpdateProviderSettings(shippingProAPICollectionSettings, dto);
            return Ok();
        }

        /// <summary>
        /// Löschen von Providereinstellungen
        /// Delete provider settings by contract id
        /// </summary>
        [HttpDelete]
        [Route("settings/provider/{contractid}")]
        [EnableCors("Intern")]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> DeleteProvider([FromRoute, BindRequired] string contractid)
        {
            applicationSettingService.DeleteProvider(shippingProAPICollectionSettings, contractid);
            return Ok();
        }

        /// <summary>
        /// Updaten der Account informationen
        /// Update account informations
        /// </summary>
        [HttpPut]
        [Route("settings/account")]
        [EnableCors("Intern")]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> UpdateAccountSettings([FromBody, BindRequired] UpdateAccountSettingsRequest dto)
        {
            ShippingProAPIAccountSettings settings = new ShippingProAPIAccountSettings()
            {
                City = dto.City,
                ContactName = dto.ContactName,
                CountryIsoA2Code = dto.CountryIsoA2Code,
                Email = dto.Email,
                Name = dto.Name,
                Name2 = dto.Name2,
                Name3 = dto.Name3,
                Phone = dto.Phone,
                PostCode = dto.PostCode,
                Street = dto.Street,
            };

            applicationSettingService.UpdateAccountSettings(shippingProAPICollectionSettings, settings);
            return Ok();
        }

        /// <summary>
        /// Cache leeren
        /// Clear Cache
        /// </summary>
        [HttpDelete]
        [Route("cache")]
        [EnableCors("Intern")]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        [ProducesResponseType(typeof(ApiErrorResponse), 500)]
        public async Task<IActionResult> ClearCache()
        {
            shippingProAPICollectionService.ResetDPDAutToken();
            return new NoContentResult();
        }

    }
}
