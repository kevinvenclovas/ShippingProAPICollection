using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ShippingProAPICollection.RestApi.Entities
{
    public class ApiErrorResponse
    {
        [Required]
        public required ApiErrorReponseCode ErrorCode { get; set; }
        [Required]
        public required string ErrorMessage { get; set; }
        public object? Payload { get; set; }
        public object? Response { get; set; }

        [SetsRequiredMembers]
        public ApiErrorResponse(ApiErrorReponseCode _errorCode, string _errorMessage, object? payload = null, object? response = null)
        {
            ErrorCode = _errorCode;
            ErrorMessage = _errorMessage;
            Payload = payload;
            Response = response;
        }


    }
}
