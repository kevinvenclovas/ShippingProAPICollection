[![Publish Docker](https://github.com/kevinvenclovas/ShippingProAPICollection/actions/workflows/publish-docker.yml/badge.svg)](https://github.com/kevinvenclovas/ShippingProAPICollection/actions/workflows/publish-docker.yml)
![Docker Image Version](https://img.shields.io/docker/v/kevinvenclovas/shippproapicollection)

[![Publish Nuget](https://github.com/kevinvenclovas/ShippingProAPICollection/actions/workflows/publish-nuget.yml/badge.svg)](https://github.com/kevinvenclovas/ShippingProAPICollection/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/ShippingProAPICollection.svg)](https://www.nuget.org/packages/ShippingProAPICollection/)
# Shipping Pro API Collection
Welcome to our C# library, designed to integrate multiple shipping service APIs into one streamlined solution. This project provides a unified interface for GLS GLS, DHL, DPD and TOF simplifying the shipping process for developers and businesses. With easy integration, you can handle logistics across different carriers seamlessly. Ideal for enhancing efficiency in e-commerce and logistics operations.

 
## Provider

- [X] GLS GLS Germany ([API Documentation](https://shipit.gls-group.eu/webservices/3_2_9/doxygen/WS-REST-API/index.html))
	- [X] Create Shipment
	- [X] Cancel Shipment
	- [X] Validate Label
	- [X] Get Estimated Delivery Days

- [X] DHL Parcel Germany (2.1.8) ([API Documentation] (https://developer.dhl.com/api-reference/parcel-de-shipping-post-parcel-germany-v2#downloads-section))
	- [X] Create Shipment
	- [X] Cancel Shipment
	
- [X] DPD Germany (4.4) ([API Documentation] (https://esolutions.dpd.com/dokumente/ShipmentService_V4_4.pdf))
	- [X] Login (2.0) ([API Documentation] (https://esolutions.dpd.com/dokumente/LoginService_V2_0.pdf))
	- [X] Create Shipment
	- [X] Cancel Shipment
	- [ ] 
- [X] Trans-o-flex
	- [X] Login
	- [X] Create Shipment
	- [X] Cancel Shipment
	
## Get Started
Before requesting shipping labels, you must first set up your specific shipping provider settings. This is achieved by injecting these settings as a singleton through dependency injection.

To finalize the setup, register the ShippingProAPICollectionService as a scoped service in your application.

	services.AddScoped<ShippingProAPICollectionService>();
	ShippingProAPICollectionSettings providerSettings = new ShippingProAPICollectionSettings()
	
#### GLS GLS
 
	GLSSettings glsSettings = new GLSSettings()
	{   
		 // PLEASE GET IN TOUCH WITH YOUR GLS CONTACT TO GET THE FOLLOWING INFORMATIONS
	 
		 // Api domain => https://shipit-wbm-test01.gls-group.eu:443
		 ApiDomain = "https://shipit-wbm-test01.gls-group.eu:443",         
		 ContactID = "276a45fkqM",       
		 Username = "276a45fkqM"
		 Password = "lXZBIF7uRccyK7Ohr64d",       
	};
	providerSettings.AddSettings("GLS", glsSettings);

#### DHL Parcel DE Shipping

	DHLSettings dhlSettings = new DHLSettings()
	{
		 // Api domain is the XXXXXXX part of your DHL api url => https://api-XXXXXXX.dhl.com/parcel/de/shipping/v2/
		 ApiDomain = "sandbox",
		 Password = "pass",
		 Username = "sandy_sandbox",
		 DHLShipmentProfile = "STANDARD_GRUPPENPROFIL",
		 InternationalAccountNumber = "33333333335301",
		 NationalAccountNumber = "33333333330102",
		 // Optional for DHL Kleinpaket / DHL small package (max. 35 x 25 x 8 cm)
		 // for DHLProductType.V62KP
		 WarenpostNationalAccountNumber = "33333333330103",
		 // Optional for Warenpost international / international premium (max. 35 x 25 x 5 cm)
		 WarenpostInternationalAccountNumber = "33333333330104",
		 LabelPrintFormat = "910-300-410",
		 // Create your DHL APP here -> https://developer.dhl.com/user/apps
		 APIKey = "",
		 APILanguage = "de-DE" // en-US or de-DE
	};
	providerSettings.AddSettings("DHL", dhlSettings);

#### DPD

	DPDSettings dpdSettings = new DPDSettings()
	{
		 // Api domain is the XXXXXXX part of your DPD api url => https://public-XXXXXXX.dpd.com/services/ShipmentService/V4_4/
	     ApiDomain = "ws-stage",
	     APILanguage = "de_DE", // en_EN or de_DE
	     DepotNumber = "0191",
	     Username = "sandboxdpd",
	     Password = "xMmshh1"
	};
	providerSettings.AddSettings("DPD", dpdSettings);

#### Trans-o-flex

	TOFSettings tofSetting = new TOFSettings()
	{
		ApiDomain = "https://ichwillnurtesten.tof.de",
		Username = configuration["TOFUser"] ?? "",
		Password = configuration["TOFPassword"] ?? "",
		CustomerNr = configuration["TOFCustomerNr"] ?? "",
	};
	providerSettings.AddSettings("TOF", tofSetting);



## Add multiple contract accounts
At times, you may need to utilize multiple contract accounts from the same provider. You can add multiple contracts by specifying a contract ID like this:

	// Setup first contract
	DPDSettings dpdContract1Settings = new DPDSettings()
	{
		 // Api domain is the XXXXXXX part of your DPD api url => https://public-XXXXXXX.dpd.com/services/ShipmentService/V4_4/
	     ApiDomain = "ws-stage",
	     APILanguage = "de_DE", // en_EN or de_DE
	     DepotNumber = "0191",
	     Username = "sandboxdpd",
	     Password = "xMmshh1"
	};
	
	providerSettings.AddSettings("DPD1", dpdContract1Settings);
	
	// Setup second contract
	DPDSettings dpdContract3Settings = new DPDSettings()
	{
		 // Api domain is the XXXXXXX part of your DPD api url => https://public-XXXXXXX.dpd.com/services/ShipmentService/V4_4/
     	 ApiDomain = "ws-stage",
     	 APILanguage = "de_DE", // en_EN or de_DE
     	 DepotNumber = "0191",
     	 Username = "sandboxdpd",
     	 Password = "xMmshh1"
	};
	providerSettings.AddSettings("DPD2", dpdSettings);

## Complete Example: Creating a Shipping Label with Custom Sender Address

This example demonstrates how to create a shipping label while overriding the default sender address. This is useful when you have multiple warehouses or need to ship from different locations.

### Setup and Configuration

First, configure your shipping service with a default sender address:

```csharp
using ShippingProAPICollection;
using ShippingProAPICollection.Models;
using ShippingProAPICollection.Provider.DHL;
using ShippingProAPICollection.Provider.DHL.Entities;

// Configure default sender address
var defaultShipFromAddress = new ShippingProAPIShipFromAddress()
{
    Name = "Main Warehouse GmbH",
    ContactName = "John Doe",
    Street = "Hauptstraße 123",
    PostCode = "12345",
    City = "Berlin",
    CountryIsoA2Code = "DE",
    Phone = "030 12345678",
    Email = "warehouse@company.com"
};

// Setup shipping settings
var settings = new ShippingProAPICollectionSettings(defaultShipFromAddress);

// Configure DHL settings
var dhlSettings = new DHLSettings()
{
    ApiDomain = "sandbox", // or "eu" for production
    Password = "your_password",
    Username = "your_username",
    DHLShipmentProfile = "STANDARD_GRUPPENPROFIL",
    InternationalAccountNumber = "33333333335301",
    NationalAccountNumber = "33333333330102",
    WarenpostNationalAccountNumber = "33333333330103",
    WarenpostInternationalAccountNumber = "33333333330104",
    LabelPrintFormat = "910-300-410",
    APIKey = "your_api_key",
    APILanguage = "de-DE"
};

settings.AddSettings("DHL", dhlSettings);

// Initialize the service
var shippingService = new ShippingProAPICollectionService(memoryCache, settings);
```

### Creating a Label with Custom Sender Address

```csharp
// Define a custom sender address (e.g., for a different warehouse)
var customShipFromAddress = new ShippingProAPIShipFromAddress()
{
    Name = "Branch Office GmbH",
    Name2 = "Abteilung Versand",
    Name3 = "Lager 2",
    ContactName = "Jane Smith",
    Street = "Nebenstraße 456",
    PostCode = "54321",
    City = "Hamburg",
    CountryIsoA2Code = "DE",
    Phone = "040 98765432",
    Email = "branch@company.com"
};

// Create the shipping request with custom sender address
var request = new DHLShipmentRequestModel("DHL")
{
    // Recipient information
    Adressline1 = "Max Mustermann",
    Adressline2 = "c/o Muster GmbH",
    Street = "Musterstraße 789",
    PostCode = "98765",
    City = "München",
    Country = "DE",
    
    // Shipping details
    ServiceProduct = DHLProductType.V01PAK,
    ServiceType = DHLServiceType.NONE,
    Items = new List<RequestShipmentItem>
    {
        new RequestShipmentItem { Weight = 2.5f }
    },
    
    // References
    InvoiceReference = "INV-2024-001",
    CustomerReference = "CUST-12345",
    
    // Contact information
    Phone = "089 12345678",
    EMail = "max.mustermann@example.com",
    WithEmailNotification = true,
    
    // Override the default sender address with custom address
    ShipFromAddress = customShipFromAddress
};

// Validate the request
request.Validate();

// Create the shipping label
try
{
    var results = await shippingService.RequestLabel(request);
    var result = results.FirstOrDefault();
    
    if (result != null && result.Label?.Length > 0)
    {
        Console.WriteLine($"Label created successfully!");
        Console.WriteLine($"Tracking Number: {result.ParcelNumber}");
        Console.WriteLine($"Cancel ID: {result.CancelId}");
        
        // Save the label PDF
        await File.WriteAllBytesAsync($"label_{result.ParcelNumber}.pdf", result.Label);
        
        Console.WriteLine($"Label saved as: label_{result.ParcelNumber}.pdf");
    }
    else
    {
        Console.WriteLine("Failed to create label");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error creating label: {ex.Message}");
}
```
