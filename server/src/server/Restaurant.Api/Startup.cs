using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Restaurant.Api.Configuration;
using Restaurant.Api.Filters;
using Restaurant.Api.Middlewares;
using Restaurant.Api.ModelBinders;
using Restaurant.Core.AuthContext.Commands;
using Restaurant.Core.AuthContext.Configuration;
using Restaurant.Domain.Connectors;
using Restaurant.Domain.Entities;
using Restaurant.Persistence.Connectors;
using Restaurant.Persistence.EntityFramework;
using Serilog;

namespace Restaurant.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
			services.AddSettings(Configuration);

            services.AddDbContext(Configuration.GetConnectionString("DefaultConnection"));

            services.AddDbConnectors();

            services.AddRepositories();

            services.AddMapper();

            services.AddSwagger();

            services.AddJwtIdentity(Configuration.GetSection(nameof(JwtConfiguration)));

            services.AddLogging(logBuilder => logBuilder.AddSerilog(dispose: true));

            services.AddMarten(Configuration);

            services.AddCqrs();

            services.AddMediatR();

            services.AddExcelWorkbook();

            services.AddGenerators();

            services.AddDatabaseLogger();

            services.AddHttpContextAccessor();

            services.AddFileLoaderServices();

            services.AddNotificationManager();

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new OptionModelBinderProvider());

                options.Filters.Add<ModelStateFilter>();

                options.Filters.Add(
                    new AsyncExceptionFilter(
                        services.BuildServiceProvider().GetRequiredService<IAsyncLogger>()));

                options.RespectBrowserAcceptHeader = true;
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterValidator>())
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ConnectionManagerBase.SetConnectionString(Configuration.GetConnectionString("DefaultConnection"));
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            ApplicationDbContext dbContext,
            UserManager<User> userManager)
        {
            app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());

            if (env.IsDevelopment())
            {
                dbContext.Database.EnsureCreated();
                app.AddDefaultAdminAccountIfNoneExisting(userManager, Configuration).Wait();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<LogMiddleware>();

            loggerFactory.AddLogging(Configuration.GetSection("Logging"));

            app.UseHttpsRedirection();

            app.UseSwagger("Restaurant");

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
