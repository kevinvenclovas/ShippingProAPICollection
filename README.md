
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
