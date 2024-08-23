using Microsoft.Extensions.DependencyInjection;
using ShippingProAPICollection.Provider;
using ShippingProAPICollection.Provider.TRANSOFLEX;

namespace ShippingProAPICollection.NUnitTests
{
    public class TOFTests : TestBase
    {
        
        /// <summary>
        /// Create one label with 0.5 Kg
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateSingleShippingLabel()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new TOFShipmentRequestModel("TOF")
            {
                Items = [new RequestShipmentItem() { Weight = 0.5f }],
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                Note1 = "",
                Note2 = "",
                ShipmentReference = "abcdefg",
                ShipmentType = Provider.TRANSOFLEX.Entities.Create.TOFShipmentType.NORMAL
            };
            request.Validate();

            var result = (await shippingCollection.RequestLabel(request));

            Assert.That(result.Count() == 1);
            Assert.That(result.FirstOrDefault()?.Label.Length > 0);
        }


        /// <summary>
        /// Create one label with 0.5 Kg
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CancelSingleShippingLabel()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new TOFShipmentRequestModel("TOF")
            {
                Items = [new RequestShipmentItem() { Weight = 0.5f }],
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                Note1 = "",
                Note2 = "",
                ShipmentReference = "abcdefg",
                ShipmentType = Provider.TRANSOFLEX.Entities.Create.TOFShipmentType.NORMAL
            };
            request.Validate();

            var result = (await shippingCollection.RequestLabel(request));

            var resultCancel = await shippingCollection.CancelLabel("TOF", result[0].CancelId);

            Assert.That(resultCancel == Models.Entities.ShippingCancelResult.CANCLED);
        }
    }
}