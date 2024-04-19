using ShippingProAPICollection.Models.Entities;
using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.RestApi.Entities.DTOs
{
   
    public class CancelShippingLabelReponse
    {
        [SetsRequiredMembers]
        public CancelShippingLabelReponse(ShippingCancelResult result)
        {
            Result = result;
        }

        public required ShippingCancelResult Result { get; set; }
    }
}
