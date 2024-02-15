using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Provider.DHL;
using ShippingProAPICollection.Provider.DPD;
using ShippingProAPICollection.Provider.GLS;
using System.Reflection;

namespace ShippingProAPICollection.NUnitTests
{
    [TestFixture]
    public class TestBase
    {
        protected ServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .Build();

            var services = new ServiceCollection();
            services.AddMemoryCache();

            ShippingProAPIAccountSettings accountSettings = new ShippingProAPIAccountSettings()
            {
                Name = "Homer Simpson",
                Street = "Simpsonstreet 1",
                ContactName = "Maggie Simpson",
                CountryIsoA2Code = "DE",
                City = "Springfield",
                Email = "homer@duffbeer.de",
                PostCode = "73479"
            };

            ShippingProAPICollectionSettings providerSettings = new ShippingProAPICollectionSettings(accountSettings);

            GLSSettings GLSSettings = new GLSSettings()
            {
                ApiDomain = "test01",
                ContactID = "276a45fkqM",
                Password = "lXZBIF7uRccyK7Ohr64d",
                Username = "276a45fkqM"
            };

            providerSettings.AddSettings("GLS", GLSSettings);

            DHLSettings dhlSettings = new DHLSettings()
            {
                ApiDomain = "sandbox",
                Password = "pass",
                Username = "sandy_sandbox",
                DHLShipmentProfile = "STANDARD_GRUPPENPROFIL",
                InternationalAccountNumber = "33333333335301",
                NationalAccountNumber = "33333333330102",
                LabelPrintFormat = "910-300-410",
                APIKey = configuration["DHLAPIKey"],
                APILanguage = "de-DE"
            };

            providerSettings.AddSettings("DHL", dhlSettings);

            DPDSettings dpdSettings = new DPDSettings()
            {
                ApiDomain = "ws-stage",
                APILanguage = "de_DE",
                DepotNumber = "0191",
                Username = "sandboxdpd",
                Password = "xMmshh1"
            };

            providerSettings.AddSettings("DPD", dpdSettings);

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
