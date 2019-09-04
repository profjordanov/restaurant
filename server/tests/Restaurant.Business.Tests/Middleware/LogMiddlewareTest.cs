using System.Threading.Tasks;
using Restaurant.Api.Middlewares;
using Restaurant.Persistence.Connectors;
using Xunit;

namespace Restaurant.Business.Tests.Middleware
{
    public class LogMiddlewareTest
    {
        [Fact]
        public async Task Test()
        {
            var mid = new LogMiddleware(
                context => Task.CompletedTask, new AsyncDatabaseLogger());
        }
    }
}