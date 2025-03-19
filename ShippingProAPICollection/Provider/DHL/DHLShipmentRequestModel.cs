using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
using ShippingProAPICollection.Provider.DHL.Entities;
using ShippingProAPICollection.Provider.DHL.Entities.VASService;

namespace ShippingProAPICollection.Provider.DHL
{
    public class DHLShipmentRequestModel : RequestShipmentBase
    {
        public DHLShipmentRequestModel(string contractID) : base(contractID)
        {

        }

        public DHLShipmentRequestModel() : base(ShippingProviderType.DHL.ToString())
        {

        }

        public override string ProviderType { get; } = ShippingProviderType.DHL.ToString();
        public override float MaxPackageWeight { get; } = 31.5f;

        /// <summary>
        /// Mit Email Notification an den Kunden -> E-Mail muss dafür angegeben werden |
        /// Should the shipping provider notify the customer on status changes? If TRUE the Email address musst be set
        /// </summary>
        /// <example>false</example>
        public bool WithEmailNotification { get; set; }

        /// <summary>
        /// Normal oder Express Versand |
        /// Normal or Express shipping
        /// </summary>
        /// <example>V01PAK</example>
        [Required]
        public required DHLProductType ServiceProduct { get; set; }

        /// <summary>
        /// Welchen Service soll für den Versand in anspruch genommen werden? Deposit-Service oder keinen? |
        /// What kind of service should be used? Deposit-Service or none
        /// </summary>
        /// <example>DEPOSIT</example>
        [Required]
        public required DHLServiceType ServiceType { get; set; }

        //Packstation 
        /// <summary>
        /// Angaben zur Packstationen falls Servicetype == PACKSTATION |
        /// Packing station information if Servicetype == PACKSTATION 
        /// </summary>
        public DHLLockerData? Locker { get; set; }

        //Postfilale 
        /// <summary>
        /// Angaben zur Postfilale falls Servicetype == POSTFILIALE |
        /// Postoffice information if Servicetype == POSTFILIALE
        /// </summary>
        public DHLPostOfficeData? PostOffice { get; set; }

        /// <summary>
        /// Optionale DHL VAS Services
        /// Optional DHL VAS Services
        /// </summary>
        public List<VASService> VASServices { get; set; } = [];

        internal override bool IsExpress()
        {
            return ServiceProduct == DHLProductType.V01PRIO;
        }

        /// <summary>
        /// Referenc string des Versandlabels auf Rechnungsreferenze und Kundenreferenze bauen
        /// Build shippinglabel ref number from invoice reference and customer reference 
        /// </summary>
        /// <returns></returns>
        internal string? GetRefString()
        {
            if (!String.IsNullOrEmpty(InvoiceReference) && String.IsNullOrEmpty(CustomerReference))
            {
                return InvoiceReference.FillString(8, '-');
            }
            else if (String.IsNullOrEmpty(InvoiceReference) && !String.IsNullOrEmpty(CustomerReference))
            {
                return CustomerReference.FillString(8, '-');
            }
            else if (!String.IsNullOrEmpty(InvoiceReference) && !String.IsNullOrEmpty(CustomerReference))
            {
                return (InvoiceReference + " | " + CustomerReference).FillString(8, '-');
            }
            return null;
        }

        public override void Validate()
        {
            base.Validate();

            if (ServiceType == DHLServiceType.DEPOSIT && !VASServices.Any(x => x.GetType() == typeof(VASPlaceOfDepositService))) throw new ShipmentRequestNotNullException("PlaceOfDepositService");
            if (ServiceType == DHLServiceType.LOCKER && Locker == null) throw new ShipmentRequestNotNullException("Locker");
            if (ServiceType == DHLServiceType.POSTOFFICE && PostOffice == null) throw new ShipmentRequestNotNullException("PostOffice");
            if (!Note1.RangeLenghtValidation(0, 35)) throw new ShipmentRequestNoValidStringLengthException("Note1", null, 35);
            if (!Phone.RangeLenghtValidation(0, 20)) throw new ShipmentRequestNoValidStringLengthException("Phone", null, 20);
            if (!StreetNumber.RangeLenghtValidation(0, 20)) throw new ShipmentRequestNoValidStringLengthException("StreetNumber", null, 20);
            if (!Adressline1.RangeLenghtValidation(1, 50)) throw new ShipmentRequestNoValidStringLengthException("Adressline1", 1, 50);
            if (!Adressline2.RangeLenghtValidation(0, 50)) throw new ShipmentRequestNoValidStringLengthException("Adressline2", null, 50);
            if (!Adressline3.RangeLenghtValidation(0, 50)) throw new ShipmentRequestNoValidStringLengthException("Adressline3", null, 50);
            if (!Street.RangeLenghtValidation(1, 50)) throw new ShipmentRequestNoValidStringLengthException("Street", 1, 50);
            if (!City.RangeLenghtValidation(1, 40)) throw new ShipmentRequestNoValidStringLengthException("City", 1, 40);
            if (!InvoiceReference.MaxLenghtValidation(35)) throw new ShipmentRequestNoValidStringLengthException("InvoiceReference", null, 35);
            if (!CustomerReference.MaxLenghtValidation(35)) throw new ShipmentRequestNoValidStringLengthException("CustomerReference", null, 35);
            if (!GetRefString().MaxLenghtValidation(35)) throw new ShipmentRequestNoValidStringLengthException("InvoiceReference + CustomerReference", null, 35);
            if (WithEmailNotification && !EMail.RangeLenghtValidation(1, 80)) throw new ShipmentRequestNoValidStringLengthException("EMail", null, 80);

            VASServices.ForEach(x => x.Validate());

        }
    }
}
