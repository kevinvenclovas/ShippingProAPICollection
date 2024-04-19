using System.ComponentModel.DataAnnotations;
using ShippingProAPICollection.Models.Utils;
using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Provider.DPD.Entities;

namespace ShippingProAPICollection.Provider.DPD
{
    public class DPDShipmentRequestModel : RequestShipmentBase
    {
        public DPDShipmentRequestModel(string contractID) : base(contractID)
        {

        }

        public DPDShipmentRequestModel() : base(ShippingProviderType.DPD.ToString())
        {

        }

        public override string ProviderType { get; } = ShippingProviderType.DPD.ToString();
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
        /// <example>CL</example>
        [Required]
        public required DPDProductType ServiceProduct { get; set; }

        /// <summary>
        /// Welchen Service soll für den Versand in anspruch genommen werden? Keiner oder Rücksendung? |
        /// What kind of service should be used? Return or none
        /// </summary>
        /// <example>SHOPRETURN</example>
        [Required]
        public required DPDServiceType ServiceType { get; set; }


        /// <summary>
        /// Ist die Lieferadresse ein Bussiness Kunde
        /// Is the delivery adress a commercial customer
        /// </summary>
        public bool DeliveryAdressIsCommercialCustomer { get; set; }


        internal override bool IsExpress()
        {
            return ServiceProduct == DPDProductType.IE2;
        }

        public override void Validate()
        {
            base.Validate();

            if (!Note1.RangeLenghtValidation(0, 70)) throw new ShipmentRequestNoValidStringLengthException("Note1", null, 70);
            if (!Adressline1.RangeLenghtValidation(1, 35)) throw new ShipmentRequestNoValidStringLengthException("Adressline1", 1, 35);
            if (!Adressline2.RangeLenghtValidation(0, 35)) throw new ShipmentRequestNoValidStringLengthException("Adressline2", null, 35);
            if (!Adressline3.RangeLenghtValidation(0, 35)) throw new ShipmentRequestNoValidStringLengthException("Adressline3", null, 35);
            if (!InvoiceReference.MaxLenghtValidation(80)) throw new ShipmentRequestNoValidStringLengthException("InvoiceReference", null, 35);
            if (!CustomerReference.MaxLenghtValidation(80)) throw new ShipmentRequestNoValidStringLengthException("CustomerReference", null, 35);
            if (!Street.RangeLenghtValidation(0, 50)) throw new ShipmentRequestNoValidStringLengthException("Street", 1, 35);
            if (!(Street + " " + StreetNumber).RangeLenghtValidation(1, 50)) throw new ShipmentRequestNoValidStringLengthException("Street + StreetNumber", 1, 35);
            if (WithEmailNotification && !EMail.RangeLenghtValidation(5, 100)) throw new ShipmentRequestNoValidStringLengthException("EMail", 5, 100);
        }

    }
}
