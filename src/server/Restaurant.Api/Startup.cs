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
using Restaurant.Api.ModelBinders;
using Restaurant.Core.Configuration;
using Restaurant.Domain.Entities;
using Restaurant.Persistence.EntityFramework;
using Serilog;

namespace Restaurant.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext(Configuration.GetConnectionString("DbConnectionString"));

            services.AddMapper();

            services.AddSwagger();

            services.AddJwtIdentity(Configuration.GetSection(nameof(JwtConfiguration)));

            services.AddLogging(logBuilder => logBuilder.AddSerilog(dispose: true));

            services.AddMarten(Configuration);

            services.AddCqrs();

            services.AddMediatR();

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new OptionModelBinderProvider());
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<ModelStateFilter>();
            })
            //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterValidator>())
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            ApplicationDbContext dbContext,
            UserManager<User> userManager)
        {
            if (env.IsDevelopment())
            {
                dbContext.Database.EnsureCreated();
                app.AddDefaultAdminAccountIfNoneExisting(userManager, Configuration).Wait();
            }
            else
            {
                app.UseHsts();
            }

            loggerFactory.AddLogging(Configuration.GetSection("Logging"));

            app.UseHttpsRedirection();
            app.UseSwagger("Restaurant");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
