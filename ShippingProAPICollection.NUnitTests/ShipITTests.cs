using Microsoft.Extensions.DependencyInjection;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Provider.ShipIT;
using ShippingProAPICollection.Provider.ShipIT.Entities;

namespace ShippingProAPICollection.NUnitTests
{
    public class ShipITTests : TestBase
    {
        /// <summary>
        /// Create one label with 0.5 Kg
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateSingleShippingLabel()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new ShipITShipmentRequestModel("GLS")
            {
                ServiceProduct = ShipITProductType.PARCEL,
                Weight = 0.5f,
                LabelCount = 1,
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = ShipITServiceType.NONE,
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

            var request = new ShipITShipmentRequestModel("GLS")
            {
                ServiceProduct = ShipITProductType.PARCEL,
                Weight = 1f,
                LabelCount = 2,
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = ShipITServiceType.NONE,
            };
            request.Validate();


            var result = (await shippingCollection.RequestLabel(request));

            Assert.That(result.Count() == 2);
            Assert.That(result.FirstOrDefault()?.Label.Length > 0);
        }

        /// <summary>
        /// Create label with deposit service
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateShippingLabelWithDepositService()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new ShipITShipmentRequestModel("GLS")
            {
                ServiceProduct = ShipITProductType.PARCEL,
                Weight = 1f,
                LabelCount = 1,
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = ShipITServiceType.DEPOSIT,
                PlaceOfDeposit = "Garden"
            };
            request.Validate();

            var result = (await shippingCollection.RequestLabel(request));

            Assert.That(result.Count() == 1);
            Assert.That(result.FirstOrDefault()?.Label.Length > 0);
        }

        /// <summary>
        /// Create label with return service
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateShippingLabelWithReturnService()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new ShipITShipmentRequestModel("GLS")
            {
                ServiceProduct = ShipITProductType.PARCEL,
                Weight = 1f,
                LabelCount = 1,
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = ShipITServiceType.SHOPRETURN,
            };
            request.Validate();

            var result = (await shippingCollection.RequestLabel(request));

            Assert.That(result.Count() == 1);
            Assert.That(result.FirstOrDefault()?.Label.Length > 0);
        }


        /// <summary>
        /// Create label with return service
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ValidateShippingLabel()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new ShipITShipmentRequestModel("GLS")
            {
                ServiceProduct = ShipITProductType.PARCEL,
                Weight = 1f,
                LabelCount = 1,
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = ShipITServiceType.SHOPRETURN,
            };
            request.Validate();

            var result = (await shippingCollection.ValidateLabel(request));

            Assert.That(result.Success);
        }

        /// <summary>
        /// Create 2 labels with deposit service and cancel both
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CancelShippingLabel()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new ShipITShipmentRequestModel("GLS")
            {
                ServiceProduct = ShipITProductType.PARCEL,
                Weight = 1,
                LabelCount = 2,
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = ShipITServiceType.DEPOSIT,
                PlaceOfDeposit = "Behind the gardenhouse",
            };
            request.Validate();

            var createResult = (await shippingCollection.RequestLabel(request));

            foreach (var label in createResult)
            {
                var cancelResult = (await shippingCollection.CancelLabel("GLS", label.CancelId));
                Assert.That(cancelResult == CancelResult.CANCLED);
            }

        }

        /// <summary>
        /// Create 2 labels with deposit service and cancel both
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetEstimatedDeliveryDays()
        {
            ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

            var request = new ShipITShipmentRequestModel("GLS")
            {
                ServiceProduct = ShipITProductType.PARCEL,
                Weight = 1,
                LabelCount = 2,
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraﬂe 10",
                PostCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = ShipITServiceType.DEPOSIT,
                PlaceOfDeposit = "Behind the gardenhouse",
            };
            request.Validate();

            var days = (await shippingCollection.GetEstimatedDeliveryDays(request));

        }
        
    }
}