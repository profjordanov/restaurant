using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Restaurant.Core;
using Restaurant.Domain;
using System.Net;

namespace Restaurant.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExceptionFilter(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        public void OnException(ExceptionContext context)
        {
            var status = (int)HttpStatusCode.InternalServerError;

            var result = _hostingEnvironment.IsDevelopment() ?
                new JsonResult(context.Exception) :
                new JsonResult(Error.Critical("An unexpected internal server error has occurred."));

            context.HttpContext.Response.StatusCode = status;
            context.Result = result;
        }
    }
}
