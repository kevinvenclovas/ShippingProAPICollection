using RestSharp;
using ShippingProAPICollection.Models.Error;

namespace ShippingProAPICollection.Models.Utils
{
    public static class HTTPReponseUtils
    {
        /// <summary>
        /// Check HTTP response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="payload"></param>
        /// <param name="response"></param>
        /// <exception cref="ShippingProviderException"></exception>
        public static void CheckHttpResponse<TException>(string message, object payload, RestResponse response) where TException : ShippingProviderException
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ShippingErrorCode errorCode = ShippingErrorCode.UNKNOW;

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    errorCode = ShippingErrorCode.BAD_REQUEST_ERROR;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    errorCode = ShippingErrorCode.UNAUTHORIZED;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    errorCode = ShippingErrorCode.INTERNAL_SERVER_ERROR;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    errorCode = ShippingErrorCode.NOT_FOUND;
                }
                else
                {
                    throw (TException)Activator.CreateInstance(typeof(TException), ShippingErrorCode.UNKNOW, response.ErrorMessage + "<------>" + response.Content, new { payload = payload, response = response.Content });
                }

                throw (TException)Activator.CreateInstance(typeof(TException), errorCode, message, new { payload = payload, response = response.Content });
            }

        }
    }
}
