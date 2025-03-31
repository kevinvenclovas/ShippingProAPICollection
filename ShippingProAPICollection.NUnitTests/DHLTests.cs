using Microsoft.Extensions.DependencyInjection;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Provider;
using ShippingProAPICollection.Provider.DHL;
using ShippingProAPICollection.Provider.DHL.Entities;
using ShippingProAPICollection.Provider.DHL.Entities.VASService;

namespace ShippingProAPICollection.NUnitTests
{
   public class DHLTests : TestBase
   {
      /// <summary>
      /// Create one label with 0.5 Kg
      /// </summary>
      /// <returns></returns>
      [Test]
      public async Task CreateSingleShippingLabel()
      {
         ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

         var request = new DHLShipmentRequestModel("DHL")
         {
            ServiceProduct = DHLProductType.V01PAK,
            Items = [new RequestShipmentItem() { Weight = 0.5f }],
            Adressline1 = "Max Mustermann",
            Country = "DE",
            City = "Ellwangen",
            Street = "Maxstraﬂe 10",
            PostCode = "73479",
            InvoiceReference = "RE-123456",
            CustomerReference = "RE-123456",
            Phone = "0123456789",
            ServiceType = DHLServiceType.NONE,
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

         var request = new DHLShipmentRequestModel("DHL")
         {
            ServiceProduct = DHLProductType.V01PAK,
            Items = [new RequestShipmentItem() { Weight = 1f }, new RequestShipmentItem() { Weight = 1f }],
            Adressline1 = "Max Mustermann",
            Country = "DE",
            City = "Ellwangen",
            Street = "Maxstraﬂe 10",
            PostCode = "73479",
            InvoiceReference = "RE-123456",
            Phone = "0123456789",
            ServiceType = DHLServiceType.NONE,
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

         var request = new DHLShipmentRequestModel("DHL")
         {
            ServiceProduct = DHLProductType.V01PAK,
            Items = [new RequestShipmentItem() { Weight = 1f }],
            Adressline1 = "Max Mustermann",
            Country = "DE",
            City = "Ellwangen",
            Street = "Maxstraﬂe 10",
            PostCode = "73479",
            InvoiceReference = "RE-123456",
            Phone = "0123456789",
            ServiceType = DHLServiceType.DEPOSIT,
            VASServices = [new VASPlaceOfDepositService("Garden")]
         };
         request.Validate();

         var result = (await shippingCollection.RequestLabel(request));

         Assert.That(result.Count() == 1);
         Assert.That(result.FirstOrDefault()?.Label.Length > 0);
      }

      /// <summary>
      /// Create label to postoffice
      /// </summary>
      /// <returns></returns>
      [Test]
      public async Task CreateShippingLabelWithPostOfficeService()
      {
         ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

         var request = new DHLShipmentRequestModel("DHL")
         {
            ServiceProduct = DHLProductType.V01PAK,
            Items = [new RequestShipmentItem() { Weight = 1f }],
            Adressline1 = "Max Mustermann",
            Country = "DE",
            City = "Ellwangen",
            Street = "Maxstraﬂe 10",
            PostCode = "73479",
            InvoiceReference = "RE-123456",
            Phone = "0123456789",
            ServiceType = DHLServiceType.POSTOFFICE,
            PostOffice = new DHLPostOfficeData()
            {
               City = "Ellwangen",
               PostCode = "73479",
               PostfilialeNumber = "564",
               PostNumber = "943864414",
            }
         };
         request.Validate();

         var result = (await shippingCollection.RequestLabel(request));

         Assert.That(result.Count() == 1);
         Assert.That(result.FirstOrDefault()?.Label.Length > 0);
      }

      /// <summary>
      /// Cancel shipping label
      /// </summary>
      /// <returns></returns>
      [Test]
      public async Task CancelShippingLabel()
      {
         ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

         var request = new DHLShipmentRequestModel("DHL")
         {
            ServiceProduct = DHLProductType.V01PAK,
            Items = [new RequestShipmentItem() { Weight = 1f }],
            Adressline1 = "Max Mustermann",
            Country = "DE",
            City = "Ellwangen",
            Street = "Maxstraﬂe 10",
            PostCode = "73479",
            InvoiceReference = "RE-123456",
            Phone = "0123456789",
            ServiceType = DHLServiceType.POSTOFFICE,
            PostOffice = new DHLPostOfficeData()
            {
               City = "Ellwangen",
               PostCode = "73479",
               PostfilialeNumber = "564",
               PostNumber = "943864414",
            }
         };
         request.Validate();

         var createResult = (await shippingCollection.RequestLabel(request));

         foreach (var label in createResult)
         {
            var cancelResult = (await shippingCollection.CancelLabel("DHL", label.CancelId));
            Assert.That(cancelResult == ShippingCancelResult.CANCELED);
         }

      }


      /// <summary>
      /// Create one label with IdentCheck
      /// </summary>
      /// <returns></returns>
      [Test]
      public async Task CreateSingleShippingLabelWithIdentCheck()
      {
         ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

         var request = new DHLShipmentRequestModel("DHL")
         {
            ServiceProduct = DHLProductType.V01PAK,
            Items = [new RequestShipmentItem() { Weight = 0.5f }],
            Adressline1 = "Max Mustermann",
            Country = "DE",
            City = "Ellwangen",
            Street = "Maxstraﬂe 10",
            PostCode = "73479",
            InvoiceReference = "RE-123456",
            CustomerReference = "RE-123456",
            Phone = "0123456789",
            ServiceType = DHLServiceType.NONE,
            VASServices = [
                 new VASIdentCheckService(
                        new DateTimeOffset(1976, 2, 1, 0, 0, 0, TimeSpan.Zero),
                        "Max",
                        "Mustermann",
                         MinimumAge.A16
                    )
             ]
         };
         request.Validate();

         var result = (await shippingCollection.RequestLabel(request));

         Assert.That(result.Count() == 1);
         Assert.That(result.FirstOrDefault()?.Label.Length > 0);
      }

      /// <summary>
      /// Create one label with ContactName
      /// </summary>
      /// <returns></returns>
      [Test]
      public async Task CreateSingleShippingWithContactName()
      {
         ShippingProAPICollectionService shippingCollection = _serviceProvider.GetRequiredService<ShippingProAPICollectionService>();

         var request = new DHLShipmentRequestModel("DHL")
         {
            ServiceProduct = DHLProductType.V01PAK,
            Items = [new RequestShipmentItem() { Weight = 0.5f }],
            Adressline1 = "Max Mustermann",
            Country = "DE",
            City = "Ellwangen",
            Street = "Maxstraﬂe 10",
            PostCode = "73479",
            ContactName = "Erika Musterfrau",
            InvoiceReference = "RE-123456",
            CustomerReference = "RE-123456",
            Phone = "0123456789",
            ServiceType = DHLServiceType.NONE,
         };
         request.Validate();

         var result = (await shippingCollection.RequestLabel(request));

         Assert.That(result.Count() == 1);
         Assert.That(result.FirstOrDefault()?.Label.Length > 0);
      }
   }
}