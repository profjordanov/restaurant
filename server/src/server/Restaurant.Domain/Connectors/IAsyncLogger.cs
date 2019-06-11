using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Domain.Connectors
{
    public interface IAsyncLogger
    {
        Task LogWarningAsync(HttpContext httpContext, CancellationToken cancellationToken);

        Task LogCriticalAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken);

        Task LogInformationAsync(HttpContext httpContext, CancellationToken cancellationToken);

        Task LogErrorAsync(HttpContext httpContext, CancellationToken cancellationToken);
    }
}