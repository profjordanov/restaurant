using Microsoft.AspNetCore.Http;
using Restaurant.Domain.Connectors;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Api.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;
        private readonly IAsyncLogger _asyncLogger;

        public LogMiddleware(RequestDelegate nextMiddleware, IAsyncLogger asyncLogger)
        {
            _nextMiddleware = nextMiddleware;
            _asyncLogger = asyncLogger ?? throw new ArgumentNullException(typeof(IAsyncLogger).FullName);
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _nextMiddleware(context);
                switch (context.Response.StatusCode)
                {
                    case 200:
                    case 201:
                        await _asyncLogger.LogInformationAsync(context, CancellationToken.None);
                        break;
                    case 204:
                        await _asyncLogger.LogWarningAsync(context, CancellationToken.None);
                        break;
                    case 400:
                    case 404:
                    case 401:
                    case 403:
                    case 409:
                        await _asyncLogger.LogErrorAsync(context, CancellationToken.None);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.Fail($"An unexpected exception has occurred: {ex.Message}!" +
                           $"Inner Exception : {ex.InnerException.Message}.");
            }
        }
    }
}
