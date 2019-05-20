using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Api;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using MediatR;
using Restaurant.Core._Base;

namespace Restaurant.Business.Tests
{
    public class AppFixture
    {
        public static readonly string BaseUrl;
        private static readonly IConfiguration _configuration;
        private static readonly IServiceScopeFactory _scopeFactory;

        static AppFixture()
        {
            BaseUrl = $"http://localhost:{GetFreeTcpPort()}";

            var webhost = Program
                .CreateWebHostBuilder(new[] { "--environment", "IntegrationTests" }, BaseUrl)
                .Build();

            webhost.Start();

            var scopeFactory = (IServiceScopeFactory)webhost.Services.GetService(typeof(IServiceScopeFactory));

            _scopeFactory = scopeFactory;

            using (var scope = scopeFactory.CreateScope())
            {
                _configuration = scope.ServiceProvider.GetService<IConfiguration>();
            }
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    await action(scope.ServiceProvider).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var result = await action(scope.ServiceProvider).ConfigureAwait(false);

                    return result;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public string GetCompleteServerUrl(string route)
        {
            route = route.TrimStart('/', '\\');
            return $"{BaseUrl}/{route}";
        }

        public Task SendManyAsync(params ICommand[] commands)
        {
            return ExecuteScopeAsync(async sp =>
            {
                var mediator = sp.GetService<IMediator>();

                foreach (var command in commands)
                {
                    await mediator.Send(command);
                }

                return Task.CompletedTask;
            });
        }

        private static HttpClient BuildHttpClient(string accessToken = null)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };

            if (!string.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            }

            return client;
        }

        private static int GetFreeTcpPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint) listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}