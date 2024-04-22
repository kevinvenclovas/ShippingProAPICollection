using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShippingProAPICollection.RestApi.Entities.Error;
using System.Net;

namespace ShippingProAPICollection.RestApi.Extensions
{
    public class ModelValidationAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errorMessages = actionContext.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

                actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                actionContext.HttpContext.Response.ContentType = "application/json";
                actionContext.Result = new JsonResult(new UnprocessableEntityResponse(errorMessages));
                return;
            }
            await next();
        }

    }
}
