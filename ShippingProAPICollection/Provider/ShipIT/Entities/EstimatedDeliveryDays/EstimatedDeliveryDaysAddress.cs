using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;

namespace ShippingProAPICollection.Provider.ShipIT.Entities.EstimatedDeliveryDays
{
    internal class EstimatedDeliveryDaysAddress
    {
        public required string Street { get; set; }
        public string? StreetNumber { get; set; }
        public required string CountryCode { get; set; }
        public required string ZIPCode { get; set; }
        public required string City { get; set; }

        public void Validate()
        {
            if (!Street.RangeLenghtValidation(3, 40)) throw new ShipmentRequestNoValidStringLengthException("Street", 3, 40);
            if (!StreetNumber.RangeLenghtValidation(0,40)) throw new ShipmentRequestNoValidStringLengthException("StreetNumber", null, 40);
            if (!CountryCode.RangeLenghtValidation(2,2)) throw new ShipmentRequestNoValidStringLengthException("Country", 2, 2);
            if (!ZIPCode.RangeLenghtValidation(1, 10)) throw new ShipmentRequestNoValidStringLengthException("PostalCode", 1, 10);
            if (!City.RangeLenghtValidation(1, 40)) throw new ShipmentRequestNoValidStringLengthException("City", 1, 40);
        }
    }
}
