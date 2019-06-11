using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Restaurant.Domain.Connectors;
using Restaurant.Domain.Entities;
using Restaurant.Persistence.EntityFramework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Connectors
{
    /// <summary>
    /// Class that saves data in the <see cref="Log"/> table.
    /// </summary>
    public class AsyncDatabaseLogger : IAsyncLogger
    {
        public Task LogWarningAsync(HttpContext httpContext, string loadTime, CancellationToken cancellationToken) =>
            LogAsync(LogLevel.Warning, httpContext, null, loadTime, cancellationToken);

        public Task LogCriticalAsync(HttpContext httpContext, Exception exception, string loadTime, CancellationToken cancellationToken) =>
            LogAsync(LogLevel.Critical, httpContext, exception, loadTime, cancellationToken);

        public Task LogInformationAsync(HttpContext httpContext, string loadTime, CancellationToken cancellationToken) =>
            LogAsync(LogLevel.Information, httpContext, null, loadTime, cancellationToken);

        public Task LogErrorAsync(HttpContext httpContext, string loadTime, CancellationToken cancellationToken) =>
            LogAsync(LogLevel.Error, httpContext, null, loadTime, cancellationToken);

        protected virtual async Task LogAsync(
            LogLevel logLevel,
            HttpContext httpContext,
            Exception exception,
            string loadTime,
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
                LoadTimeInMilliseconds = loadTime
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

            await LogAsync(httpContext, log, cancellationToken);
        }

        protected virtual async Task LogAsync(HttpContext httpContext, Log log, CancellationToken cancellationToken)
        {
            using (var serviceScope = httpContext.RequestServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();

                if (httpContext.User.Identity.IsAuthenticated)
                {
                    var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                    var identityUser = await userManager.GetUserAsync(httpContext.User);
                    log.Username = identityUser.Email;
                }

                await dbContext.Logs.AddAsync(log, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}