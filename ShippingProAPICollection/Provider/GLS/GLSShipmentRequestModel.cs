using System.ComponentModel.DataAnnotations;
using ShippingProAPICollection.Models.Utils;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Provider.GLS.Entities;

namespace ShippingProAPICollection.Provider.GLS
{
    public class GLSShipmentRequestModel : RequestShipmentBase
    {
        public GLSShipmentRequestModel(string contractID) : base(contractID)
        {

        }

        public GLSShipmentRequestModel() : base(ShippingProviderType.GLS.ToString())
        {

        }

        public override string ProviderType { get; } = ShippingProviderType.GLS.ToString();
        public override float MaxPackageWeight { get; } = 31.5f;

        /// <summary>
        /// Mit Email Notification an den Kunden -> Email muss dafür angegeben werden |
        /// Should the shipping provider notify the customer on status changes? If TRUE the Email address musst be set
        /// </summary>
        /// <example>false</example>
        public bool WithEmailNotification { get; set; }

        /// <summary>
        /// Normal oder Express Versand |
        /// Normal or Express shipping
        /// </summary>
        /// <example>PARCEL</example>
        [Required]
        public required GLSProductType ServiceProduct { get; set; }

        /// <summary>
        /// Welchen Service soll für den Versand in anspruch genommen werden? Deposit-Service oder keinen? |
        /// What kind of service should be used? Deposit-Service or none
        /// </summary>
        /// <example>DEPOSIT</example>
        [Required]
        public required GLSServiceType ServiceType { get; set; }

        /// <summary>
        /// Ablageort des Paketes -> z.B Briefksten/ Hinter Blumentopf |
        /// Deposit location of the package if ServiceType = DEPOSIT 
        /// </summary>
        /// <example>Briefkasten</example>
        public string? PlaceOfDeposit { get; set; }

        /// <summary>
        /// Notiz 2 auf dem Label |
        /// Note 2 printed on the label
        /// </summary>
        /// <example>Danke für Ihre Bestellung!</example>
        public string? Note2 { get; set; }

        /// <summary>
        /// Referencenummer für das Paket |
        /// Some reference numbers printed on the label
        /// </summary>
        /// <example>RE 123456;K.Nr 5897143</example>
        public string? ShipmentReference { get; set; }

        internal override bool IsExpress()
        {
            return ServiceProduct == GLSProductType.EXPRESS;
        }

        public override void Validate()
        {
            base.Validate();

            // Check base parameters
            if (!Adressline1.RangeLenghtValidation(1, 40)) throw new ShipmentRequestNoValidStringLengthException("Adressline1", 1, 40);
            if (!Adressline2.RangeLenghtValidation(0, 40)) throw new ShipmentRequestNoValidStringLengthException("Adressline2", 1, 40);
            if (!Adressline3.RangeLenghtValidation(0, 40)) throw new ShipmentRequestNoValidStringLengthException("Adressline3", 1, 40);
            if (!InvoiceReference.MaxLenghtValidation(80)) throw new ShipmentRequestNoValidStringLengthException("InvoiceReference", null, 80);
            if (!CustomerReference.MaxLenghtValidation(80)) throw new ShipmentRequestNoValidStringLengthException("CustomerReference", null, 80);
            if (!Street.RangeLenghtValidation(3, 40)) throw new ShipmentRequestNoValidStringLengthException("Street", 3, 40);

            if (WithEmailNotification && !EMail.MaxLenghtValidation(40)) throw new ShipmentRequestNoValidStringLengthException("EMail", null, 40);
            if (!Note1.RangeLenghtValidation(0, 60)) throw new ShipmentRequestNoValidStringLengthException("Note1", null, 60);
            if (!Note2.RangeLenghtValidation(0, 60)) throw new ShipmentRequestNoValidStringLengthException("Note2", null, 60);
            if (ServiceType == GLSServiceType.DEPOSIT && !PlaceOfDeposit.RangeLenghtValidation(1, 60)) throw new ShipmentRequestNoValidStringLengthException("PlaceOfDeposit", 1, 60);
            if (!ShipmentReference.MaxLenghtValidation(80)) throw new ShipmentRequestNoValidStringLengthException("ShipmentReference", null, 25);
        }

    }
}
