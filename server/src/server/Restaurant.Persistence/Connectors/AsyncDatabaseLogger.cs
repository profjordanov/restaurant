using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Connectors;
using Restaurant.Domain.Entities;
using Restaurant.Persistence.EntityFramework;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurant.Persistence.Connectors
{
    public class AsyncDatabaseLogger : IAsyncLogger
    {
        public Task LogWarningAsync(HttpContext httpContext, CancellationToken cancellationToken) =>
            LogAsync(LogLevel.Warning, httpContext, null, cancellationToken);

        public Task LogCriticalAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) =>
            LogAsync(LogLevel.Critical, httpContext, exception, cancellationToken);

        public Task LogInformationAsync(HttpContext httpContext, CancellationToken cancellationToken) =>
            LogAsync(LogLevel.Information, httpContext, null, cancellationToken);

        public Task LogErrorAsync(HttpContext httpContext, CancellationToken cancellationToken) =>
            LogAsync(LogLevel.Error, httpContext, null, cancellationToken);

        protected virtual async Task LogAsync(
            LogLevel logLevel,
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var log = new Log
            {
                LogLevel = logLevel.ToString(),
                Date = DateTime.Now,
                Url = httpContext.Request.Path,
                HttpMethod = httpContext.Request.Method,
                HttpStatusCode = httpContext.Response.StatusCode,
                Host = httpContext.Connection.RemoteIpAddress.ToString(),
                Port = int.Parse(httpContext.Connection.RemotePort.ToString()),
            };

            if (httpContext.Request.QueryString.HasValue)
            {
                log.QueryString = httpContext.Request.QueryString.Value;
            }

            if (exception != null)
            {
                log.ExceptionType = exception.GetType().Name;
                log.ExceptionMessage = exception.Message;
            }

            var genericIdentity = httpContext.User?.Identities.OfType<GenericIdentity>().FirstOrDefault();
            if (genericIdentity != null)
            {
                log.Username = genericIdentity.Name;
            }

            await LogAsync(httpContext, log, cancellationToken);
        }

        protected virtual async Task LogAsync(HttpContext httpContext, Log log, CancellationToken cancellationToken)
        {
            using (var serviceScope = httpContext.RequestServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                await dbContext.Logs.AddAsync(log, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}