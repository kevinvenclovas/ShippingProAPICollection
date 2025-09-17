using ShippingProAPICollection.Models.Entities;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
using ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create;
using System.Text.RegularExpressions;

namespace ShippingProAPICollection.Provider.TRANSOFLEX
{
    public class TOFShipmentRequestModel : RequestShipmentBase
    {
        public TOFShipmentRequestModel(string contractID) : base(contractID)
        {

        }

        public TOFShipmentRequestModel() : base(ShippingProviderType.TRANSOFLEX.ToString())
        {

        }

        public override string ProviderType { get; } = ShippingProviderType.TRANSOFLEX.ToString();
        public override float MaxPackageWeight { get; set; } = 32.0f;

        /// <summary>
        /// Typ der Sendung | NORMAL oder PICKUP
        /// Type of the shipment | NORMAL or PICKUP
        /// </summary>
        /// <example>NORMAL</example>
        public required TOFShipmentType ShipmentType { get; set; }

        /// <summary>
        /// Info für den Versanddientleister
        /// Some information for the provider
        /// </summary>
        /// <example>2. Stock 2 Türe links</example>
        public string? ShipmentOperationInfo { get; set; }

        /// <summary>
        /// Refrenznummer der Lieferung. Wird in einen Barcode konventiert. Keine Sonderzeichen außer Bindestrich zulässig
        /// Shipment reference. Must be alphanumeric without any special character except hyphen
        /// </summary>
        /// <example>123456789</example>
        public required string ShipmentReference { get; set; }

        /// <summary>
        /// Notiz 2 auf dem Label |
        /// Note 2 printed on the label
        /// </summary>
        /// <example>Ware auf den Briefkasten</example>
        public string Note2 { get; set; }

        public override void Validate()
        {
            base.Validate();

            // Check base parameters
            if (!Adressline1.RangeLenghtValidation(1, 40)) throw new ShipmentRequestNoValidStringLengthException("Adressline1", 1, 40);
            if (!Adressline2.RangeLenghtValidation(0, 40)) throw new ShipmentRequestNoValidStringLengthException("Adressline2", 1, 40);
            if (!Adressline3.RangeLenghtValidation(0, 40)) throw new ShipmentRequestNoValidStringLengthException("Adressline3", 1, 40);
            if (!Street.RangeLenghtValidation(3, 40)) throw new ShipmentRequestNoValidStringLengthException("Street", 3, 40);

            if (!ShipmentReference.MaxLenghtValidation(15)) throw new ShipmentRequestNoValidStringLengthException("ShipmentReference", null, 15);
            if (!Note1.RangeLenghtValidation(0, 15)) throw new ShipmentRequestNoValidStringLengthException("Note1", null, 15);
            if (!Note2.RangeLenghtValidation(0, 15)) throw new ShipmentRequestNoValidStringLengthException("Note2", null, 15);

            string alphanumericPattern = @"^[a-zA-Z0-9-]+$";
            if (!Regex.IsMatch(ShipmentReference, alphanumericPattern)) throw new ShipmentRequestNoValidStringFormatException("ShipmentReference");
            if (!String.IsNullOrEmpty(Note1) && !Regex.IsMatch(Note1, alphanumericPattern)) throw new ShipmentRequestNoValidStringFormatException("Note1");
            if (!String.IsNullOrEmpty(Note2) && !Regex.IsMatch(Note2, alphanumericPattern)) throw new ShipmentRequestNoValidStringFormatException("Note2");
        }

    }

}
