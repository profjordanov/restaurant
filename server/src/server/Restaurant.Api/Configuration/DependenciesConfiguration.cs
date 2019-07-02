using AutoMapper;
using Marten;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Restaurant.Api.OperationFilters;
using Restaurant.Business._Base;
using Restaurant.Business.AuthContext;
using Restaurant.Business.ReportContext.Generators;
using Restaurant.Core.AuthContext;
using Restaurant.Core.AuthContext.Configuration;
using Restaurant.Domain.Connectors;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Events._Base;
using Restaurant.Domain.Events.Restaurant;
using Restaurant.Domain.FileLoaders;
using Restaurant.Domain.Repositories;
using Restaurant.Persistence.Connectors;
using Restaurant.Persistence.EntityFramework;
using Restaurant.Persistence.FileLoaders;
using Restaurant.Persistence.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MappingProfile = Restaurant.Core.AuthContext.MappingProfile;

namespace Restaurant.Api.Configuration
{
    public static class DependenciesConfiguration
    {
        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseNpgsql(connectionString)
                    .EnableSensitiveDataLogging());
        }

        public static void AddDbConnectors(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IQueryDbConnector, QueryDbConnector>();
        }

        public static void AddExcelWorkbook(this IServiceCollection services)
        {
            services.AddTransient<IWorkbook, XSSFWorkbook>();
        }

        public static void AddGenerators(this IServiceCollection services)
        {
            services.AddTransient<UserLoginsReportGenerator>();
        }

        public static void AddJwtIdentity(this IServiceCollection services, IConfigurationSection jwtConfiguration)
        {
            services.AddTransient<IJwtFactory, JwtFactory>();

            services.AddIdentity<User, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var signingKey = new SymmetricSecurityKey(
                Encoding.Default.GetBytes(jwtConfiguration["Secret"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtConfiguration[nameof(JwtConfiguration.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.Configure<JwtConfiguration>(options =>
            {
                options.Issuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)];
                options.Audience = jwtConfiguration[nameof(JwtConfiguration.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.AddPolicy(AuthConstants.Policies.IsAdmin, pb => pb.RequireClaim(AuthConstants.ClaimTypes.IsAdmin));
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                var documentationPath = Path.Combine(AppContext.BaseDirectory, "Restaurant.Api.Documentation.xml");

                if (File.Exists(documentationPath))
                {
                    setup.IncludeXmlComments(documentationPath);

                    var securityScheme = new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Enter 'Bearer {token}' (don't forget to add 'bearer') into the field below.",
                        Name = "Authorization",
                        Type = "apiKey"
                    };

                    setup.AddSecurityDefinition("Bearer", securityScheme);

                    setup.SwaggerDoc("v1", new Info { Title = "Restaurant.Api", Version = "v1" });

                    setup.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                    {
                        { "Bearer", Enumerable.Empty<string>() },
                    });
                }

                setup.OperationFilter<OptionOperationFilter>();
            });
        }

        public static void AddCqrs(this IServiceCollection services)
        {
            services.AddScoped<IEventBus, EventBus>();
        }

        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfiles(typeof(MappingProfile).Assembly);
            });
        }

        public static void AddCommonServices(this IServiceCollection services)
        {
        }

        public static void AddFileLoaderServices(this IServiceCollection services)
        {
            services.AddTransient<ICsvFileLoader, CsvFileLoader>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRestaurantRepository, RestaurantRepository>();
            services.AddTransient<ITownRepository, TownRepository>();
            services.AddTransient<IRatingRepository, RatingRepository>();
            services.AddTransient<IMealRepository, MealRepository>();
            services.AddTransient<IMealTypeRepository, MealTypeRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
        }

        public static void AddMarten(this IServiceCollection services, IConfiguration configuration)
        {
            var documentStore = DocumentStore.For(options =>
            {
                var config = configuration.GetSection("EventStore");
                var connectionString = config.GetValue<string>("ConnectionString");
                var schemaName = config.GetValue<string>("Schema");

                options.Connection(connectionString);
                options.AutoCreateSchemaObjects = AutoCreate.All;
                options.Events.DatabaseSchemaName = schemaName;
                options.DatabaseSchemaName = schemaName;

                options.Events.InlineProjections.AggregateStreamsWith<User>();
                options.Events.InlineProjections.AggregateStreamsWith<Domain.Entities.Restaurant>();
                options.Events.InlineProjections.AggregateStreamsWith<Rating>();
                options.Events.InlineProjections.AggregateStreamsWith<Meal>();
                options.Events.InlineProjections.AggregateStreamsWith<Order>();

                //options.Events.InlineProjections.Add(new TabViewProjection());

                var events = typeof(RestaurantRegistered)
                  .Assembly
                  .GetTypes()
                  .Where(t => typeof(IEvent).IsAssignableFrom(t))
                  .ToList();

                options.Events.AddEventTypes(events);
            });

            services.AddSingleton<IDocumentStore>(documentStore);

            services.AddScoped(sp => sp.GetService<IDocumentStore>().OpenSession());
        }

        public static void AddDatabaseLogger(this IServiceCollection services)
        {
            services.AddTransient<IAsyncLogger, AsyncDatabaseLogger>();
        }

        public static void AddNotificationManager(this IServiceCollection services)
        {
            services.AddScoped<INotificationManager, NotificationManager>();
        }
    }
}
