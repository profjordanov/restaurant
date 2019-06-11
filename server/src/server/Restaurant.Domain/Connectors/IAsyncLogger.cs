using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Domain.Connectors
{
    public interface IAsyncLogger
    {
        Task LogWarningAsync(HttpContext httpContext, string loadTime, CancellationToken cancellationToken);

        Task LogCriticalAsync(HttpContext httpContext, Exception exception, string loadTime, CancellationToken cancellationToken);

        Task LogInformationAsync(HttpContext httpContext, string loadTime, CancellationToken cancellationToken);

        Task LogErrorAsync(HttpContext httpContext, string loadTime, CancellationToken cancellationToken);
    }
}