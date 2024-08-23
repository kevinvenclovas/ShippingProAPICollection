using ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Validation;

namespace ShippingProAPICollection.Provider.TRANSOFLEX.Entities.Create.Response
{
    public class CreatedShipmentResponse
    {
        public required string AvisoShipmentId { get; set; }
        public required List<string> ParcelIds { get; set; }
        public required List<ValidationMessageReponse> ValidationMessages { get; set; }
    }

}
