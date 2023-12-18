using Microsoft.Extensions.DependencyInjection;
using ShippingProAPICollection.ShipIT;
using ShippingProAPICollection.ShipIT.Entities;

namespace ShippingProAPICollection.NUnitTests
{
    public class ShipIT : TestBase
    {
        /// <summary>
        /// Create one label with 0.5kg
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CreateGLSLabel()
        {
            ShipITShipmentService shipITService = _serviceProvider.GetRequiredService<ShipITShipmentService>();

            var request = new ShipITShipmentRequestModel()
            {
                ServiceProduct = ShipITProductType.PARCEL,
                Weight = 0.5,
                LabelCount = 1,
                Adressline1 = "Max Mustermann",
                Country = "DE",
                City = "Ellwangen",
                Street = "Maxstraße 10",
                ZIPCode = "73479",
                InvoiceReference = "RE-123456",
                Phone = "0123456789",
                ServiceType = ShipITServiceType.NONE,
                IsTestLabel = true,
            };
            request.Validate();


            var result = (await shipITService.CreateLabel(request));

            Assert.That(result.Count() == 1);
            Assert.That(result.FirstOrDefault()?.Label.Length > 0);
        }

    }
}