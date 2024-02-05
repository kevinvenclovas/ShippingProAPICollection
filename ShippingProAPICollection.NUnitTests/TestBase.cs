using Microsoft.Extensions.DependencyInjection;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.ShipIT;
using ShippingProAPICollection.ShipIT.Entities;

namespace ShippingProAPICollection.NUnitTests
{
    [TestFixture]
    public class TestBase
    {
        protected ServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var services = new ServiceCollection();

            ShippingProAPIAccountSettings accountSettins = new ShippingProAPIAccountSettings();
            services.AddSingleton<ShippingProAPIAccountSettings>(accountSettins);

            ShippingProAPICollectionSettings providerSettings = new ShippingProAPICollectionSettings();

            ShipITSettings shipItSettings = new ShipITSettings()
            {
                ApiDomain = "test01",
                ContactID = "276a45fkqM",
                LabelFormat = ShipITLabelFormat.PDF,
                Password = "lXZBIF7uRccyK7Ohr64d",
                Username = "276a45fkqM"
            };

            providerSettings.AddSettings("GLS", shipItSettings);

            services.AddSingleton(providerSettings);

            services.AddScoped<ShippingProAPICollectionService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            _serviceProvider.Dispose();
        }
    }

}
