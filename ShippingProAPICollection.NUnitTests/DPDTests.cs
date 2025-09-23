using Microsoft.Extensions.DependencyInjection;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Provider;
using ShippingProAPICollection.Provider.DPD;
using ShippingProAPICollection.Provider.DPD.Entities;

namespace ShippingProAPICollection.NUnitTests
{
    public class DPDTests : TestBase
    {
       
        /// <summary>
        /// Create one label with 0.5 Kg
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateSingleShippingLabel()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new DPDShipmentRequestModel("DPD")
            {
                ServiceProduct = DPDProductType.CL,
                Items = [new RequestShipmentItem() { Weight = 0.5f }],
                Addressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = DPDServiceType.NONE,
            };
            request.Validate();

            var result = (await shippingCollection.RequestLabel(request));

            Assert.That(result.Count() == 1);
            Assert.That(result.FirstOrDefault()?.Label.Length > 0);
        }

        /// <summary>
        /// Create two label with each 1 Kg
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateMultipleShippingLabel()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new DPDShipmentRequestModel("DPD")
            {
                ServiceProduct = DPDProductType.CL,
                Items = [new RequestShipmentItem() { Weight = 1f }, new RequestShipmentItem() { Weight = 1f }],
                Addressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = DPDServiceType.NONE,
            };
            request.Validate();

            var result = (await shippingCollection.RequestLabel(request));

            File.WriteAllBytes("merged.pdf", result.FirstOrDefault()?.Label);

            Assert.That(result.Count() == 2);
            Assert.That(result.FirstOrDefault()?.Label.Length > 0);
        }

        
        /// <summary>
        /// Create 2 labels with deposit service and cancel both
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CancelShippingLabel()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new DPDShipmentRequestModel("DPD")
            {
                ServiceProduct = DPDProductType.CL,
                Items = [new RequestShipmentItem() { Weight = 1f }],
                Addressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = DPDServiceType.NONE,
            };
            request.Validate();

            var createResult = (await shippingCollection.RequestLabel(request));

            foreach (var label in createResult)
            {
                var cancelResult = (await shippingCollection.CancelLabel("DPD", label.CancelId));
                Assert.That(cancelResult == ShippingCancelResult.CANCELED);
            }

        }
    }
}