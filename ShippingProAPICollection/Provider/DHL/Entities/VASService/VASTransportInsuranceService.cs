using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.Models.Utils;
using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.Provider.DHL.Entities.VASService
{
   public class VASTransportInsuranceService : VASService
   {
      public required string Currency { get; set; }
      public required double Value { get; set; }

      [SetsRequiredMembers]
      public VASTransportInsuranceService(string currency, double value)
      {
         Currency = currency;
         Value = value;
      }

      public override void Validate()
      {
         base.Validate();
         if (Value > 25000) throw new ShipmentRequestNoValidStringLengthException("additionalInsurance", null, 25000);
         if (!Currency.RangeLenghtValidation(3, 3)) throw new ShipmentRequestNoValidStringLengthException("additionalInsurance", 3, 3);
      }
   }
}
