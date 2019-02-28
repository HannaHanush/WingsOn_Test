using System.Net;
using Microsoft.AspNetCore.Mvc;
using WingsOn.Api.Models.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WingsOn.Api.Infrastructure
{
    public class WingsOnExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public WingsOnExceptionFilterAttribute()
        {
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is WingsOnNotFoundException)
            {
                PrepareResult(context, (int)HttpStatusCode.NotFound);
            }
            else
            {
                PrepareResult(context, (int)HttpStatusCode.InternalServerError);
            }
        }

        private void PrepareResult(ExceptionContext context, int statusCode)
        {
            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new ObjectResult(new
            {
                ErrorMessage = context.Exception.Message
            });
        }
    }
}
