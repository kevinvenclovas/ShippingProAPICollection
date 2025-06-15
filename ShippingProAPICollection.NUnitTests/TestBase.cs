using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Provider.DHL;
using ShippingProAPICollection.Provider.DPD;
using ShippingProAPICollection.Provider.GLS;
using ShippingProAPICollection.Provider.TRANSOFLEX;
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

            ShippingProAPIShipFromAddress accountSettings = new ShippingProAPIShipFromAddress()
            {
                Name = "Homer Simpson",
                Street = "Simpsonstreet 1",
                ContactName = "Maggie Simpson",
                CountryIsoA2Code = "DE",
                City = "Springfield",
                Email = "homer@duffbeer.de",
                PostCode = "73479",
                Phone = "123456789"
            };

            ShippingProAPICollectionSettings providerSettings = new ShippingProAPICollectionSettings(accountSettings);

            GLSSettings glsSettings = new GLSSettings()
            {
                ApiDomain = "https://shipit-wbm-test01.gls-group.eu:443",
                ContactID = "276a45fkqM",
                Password = "lXZBIF7uRccyK7Ohr64d",
                Username = "276a45fkqM"
            };

            providerSettings.AddSettings("GLS", glsSettings);

            DHLSettings dhlSettings = new DHLSettings()
            {
                ApiDomain = "sandbox",
                Password = "SandboxPasswort2023!",
                Username = "user-valid",
                DHLShipmentProfile = "STANDARD_GRUPPENPROFIL",
                InternationalAccountNumber = "33333333335301",
                NationalAccountNumber = "33333333330102",
                LabelPrintFormat = "910-300-410",
                APIKey = configuration["DHLAPIKey"] ?? "",
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

            TOFSettings tofSetting = new TOFSettings()
            {
                ApiDomain = "https://ichwillnurtesten.tof.de",
                Username = configuration["TOFUser"] ?? "",
                Password = configuration["TOFPassword"] ?? "",
                CustomerNr = configuration["TOFCustomerNr"] ?? "",
            };

            providerSettings.AddSettings("TOF", tofSetting);

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
