using Microsoft.Extensions.DependencyInjection;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.ShipIT;
using ShippingProAPICollection.ShipIT.Entities;

namespace ShippingProAPICollection.NUnitTests
{
    public class ShipITTests : TestBase
    {
        /// <summary>
        /// Create one label with 0.5kg
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task RequestLabel()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new ShipITShipmentRequestModel()
            {
                ServiceProduct = ShipITProductType.PARCEL,
                Weight = 0.5,
                LabelCount = 1,
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                ZIPCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = ShipITServiceType.NONE,
                IsTestLabel = true,
            };
            request.Validate();


            var result = (await shippingCollection.RequestLabel(request));

            Assert.That(result.Count() == 1);
            Assert.That(result.FirstOrDefault()?.Label.Length > 0);
        }


        /// <summary>
        /// Create 2 labels with deposit service and cancel both
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CancelLabel()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new ShipITShipmentRequestModel()
            {
                ServiceProduct = ShipITProductType.PARCEL,
                Weight = 1,
                LabelCount = 2,
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                ZIPCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = ShipITServiceType.DEPOSIT,
                PlaceOfDeposit = "Behind the gardenhouse",
                IsTestLabel = true,
            };
            request.Validate();

            var createResult = (await shippingCollection.RequestLabel(request));

            foreach (var label in createResult)
            {
                var cancelResult = (await shippingCollection.CancelLabel(ProviderType.SHIPIT, label.CancelId));
                Assert.That(cancelResult == CancelResult.CANCLED);
            }

        }
    }
}