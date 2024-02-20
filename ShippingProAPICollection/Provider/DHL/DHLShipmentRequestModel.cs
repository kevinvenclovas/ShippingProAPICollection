using ShippingProAPICollection.Models.Utils;
using System.ComponentModel.DataAnnotations;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Provider.DHL.Entities;

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

        //Deposit 
        /// <summary>
        /// Ablageort des Paketes falls Servicetype == DEPOSIT |
        /// Deposit location of the package if ServiceType = DEPOSIT 
        /// </summary>
        /// <example>Briefksten/ Hinter Blumentopf</example>
        [MaxLength(60)]
        public string? PlaceOfDeposit { get; set; }

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

        internal override bool IsExpress()
        {
            return ServiceProduct == DHLProductType.V01PRIO;
        }

        public override void Validate()
        {
            base.Validate();

            if (ServiceType == DHLServiceType.DEPOSIT && !PlaceOfDeposit.RangeLenghtValidation(1, 60)) throw new ShipmentRequestNoValidStringLengthException("PlaceOfDeposit", 1, 60);
            if (ServiceType == DHLServiceType.LOCKER && Locker == null) throw new ShipmentRequestNotNullException("Locker");
            if (ServiceType == DHLServiceType.POSTOFFICE && PostOffice == null) throw new ShipmentRequestNotNullException("PostOffice");
            if (!Note1.RangeLenghtValidation(0, 60)) throw new ShipmentRequestNoValidStringLengthException("Note1", 1, 60);
            if (!Adressline1.RangeLenghtValidation(1, 50)) throw new ShipmentRequestNoValidStringLengthException("Adressline1", 1, 50);
            if (!Adressline2.RangeLenghtValidation(0, 50)) throw new ShipmentRequestNoValidStringLengthException("Adressline2", null, 50);
            if (!Adressline3.RangeLenghtValidation(0, 50)) throw new ShipmentRequestNoValidStringLengthException("Adressline3", null, 50);
            if (!Street.RangeLenghtValidation(1, 50)) throw new ShipmentRequestNoValidStringLengthException("Street", 1, 50);
            if (!City.RangeLenghtValidation(1, 40)) throw new ShipmentRequestNoValidStringLengthException("City", 1, 40);
            if (!InvoiceReference.MaxLenghtValidation(35)) throw new ShipmentRequestNoValidStringLengthException("InvoiceReference", null, 35);
            if (!CustomerReference.MaxLenghtValidation(35)) throw new ShipmentRequestNoValidStringLengthException("CustomerReference", null, 35);
            if (WithEmailNotification && !EMail.RangeLenghtValidation(1, 80)) throw new ShipmentRequestNoValidStringLengthException("EMail", null, 80);
            if (!String.IsNullOrEmpty(InvoiceReference) && !InvoiceReference.RangeLenghtValidation(8, 35)) throw new ShipmentRequestNoValidStringLengthException("InvoiceReference", 8, 35);
        }
    }
}
