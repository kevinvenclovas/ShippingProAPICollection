# Shipping Pro API Collection
Welcome to our C# library, designed to integrate multiple shipping service APIs into one streamlined solution. This project provides a unified interface for GLS Shipit, DHL, and DPD, simplifying the shipping process for developers and businesses. With easy integration, you can handle logistics across different carriers seamlessly. Ideal for enhancing efficiency in e-commerce and logistics operations.

## Coming Provider

- [ ] GLS ShipIT Germany ([API Documentation](https://shipit.gls-group.eu/webservices/3_2_9/doxygen/WS-REST-API/index.html))
	- [X] Create Shipment
	- [ ] Cancel Shipment

- [ ] DHL Parcel Germany
	- [ ] Create Shipment
	- [ ] Cancel Shipment
	
- [ ] DPD Germany
	- [ ] Create Shipment
	- [ ] Cancel Shipment

## Get Started
Before requesting shipping labels, you must first set up your specific shipping provider settings. This is achieved by injecting these settings as a singleton through dependency injection.

#### GLS ShipIT
 
	ShipITSettings shipItSettings = new ShipITSettings()
	{   
		 // Define your desired label result format
		 LabelFormat = ShipITLabelFormat.PDF,
	
		 // PLEASE GET IN TOUCH WITH YOUR GLS CONTACT TO GET THE FOLLOWING INFORMATIONS
	 
		 // Api domain is the XXXXXXX part of your GLS-ShipIT api url => https://shipit-wbm-XXXXXXX.gls-group.eu:443/backend/rs
		 ApiDomain = "test01",         
		 ContactID = "276a45fkqM",       
		 Username = "276a45fkqM"
		 Password = "lXZBIF7uRccyK7Ohr64d",       
	};
