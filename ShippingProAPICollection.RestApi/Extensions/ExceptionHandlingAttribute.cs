using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShippingProAPICollection.Models.Error;
using ShippingProAPICollection.RestApi.Entities.Error;
using System.Net;

namespace ShippingProAPICollection.RestApi.Extensions
{
    public class ExceptionHandlingAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var actionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            Type controllerType = actionDescriptor.ControllerTypeInfo;

            var controllerBase = typeof(ControllerBase);
            var controller = typeof(Controller);

            // Api's implements ControllerBase but not Controller
            if (controllerType.IsSubclassOf(controllerBase) && !controllerType.IsSubclassOf(controller))
            {
                switch (context.Exception)
                {
                    case ShippingProviderException shippingProviderException:

                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.HttpContext.Response.ContentType = "application/json";
                        context.Result = new JsonResult(new BadRequestReponse(shippingProviderException));
                        break;
                    default:

                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.HttpContext.Response.ContentType = "application/json";
                        if (context.Exception != null)
                        {
                            context.Result = new JsonResult(new InternalServerErrorReponse(context.Exception.Message));
                        }
                        break;
                }

            }

        }

    }
}
