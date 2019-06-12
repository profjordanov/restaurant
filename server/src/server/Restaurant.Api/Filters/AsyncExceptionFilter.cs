using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Restaurant.Domain.Connectors;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Api.Filters
{
    public class AsyncExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IAsyncLogger _asyncLogger;

        public AsyncExceptionFilter(IAsyncLogger asyncLogger)
        {
            _asyncLogger = asyncLogger ?? throw new ArgumentNullException(typeof(IAsyncLogger).FullName);
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (context.Exception.GetType() == typeof(OperationCanceledException)
               || context.Exception.GetType() == typeof(TaskCanceledException))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            }

            context.Result = new ObjectResult(context.Exception);
            await _asyncLogger.LogCriticalAsync(context.HttpContext, context.Exception, null, CancellationToken.None);
        }
    }
}