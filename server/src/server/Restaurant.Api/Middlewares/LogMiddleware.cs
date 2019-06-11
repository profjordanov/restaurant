using Microsoft.AspNetCore.Http;
using Restaurant.Domain.Connectors;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Api.Middlewares
{
    /// <summary>
    /// Middleware that logs each request and some other info such as IP addresses to the database.
    /// </summary>
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                await _nextMiddleware(context);
                switch (context.Response.StatusCode)
                {
                    case 200:
                    case 201:
                        stopwatch.Stop();
                        await _asyncLogger.LogInformationAsync(context, stopwatch.ElapsedMilliseconds.ToString(), CancellationToken.None);
                        break;
                    case 204:
                        stopwatch.Stop();
                        await _asyncLogger.LogWarningAsync(context, stopwatch.ElapsedMilliseconds.ToString(), CancellationToken.None);
                        break;
                    case 400:
                    case 404:
                    case 401:
                    case 403:
                    case 409:
                        stopwatch.Stop();
                        await _asyncLogger.LogErrorAsync(context, stopwatch.ElapsedMilliseconds.ToString(), CancellationToken.None);
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
