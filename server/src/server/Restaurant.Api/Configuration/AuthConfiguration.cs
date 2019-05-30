using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Restaurant.Core.AuthContext;
using Restaurant.Domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Restaurant.Api.Configuration
{
    public static class AuthConfiguration
    {
        public static async Task AddDefaultAdminAccountIfNoneExisting(
            this IApplicationBuilder app,
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            var adminSection = configuration.GetSection("DefaultAdminAccount");

            var adminEmail = adminSection["Email"];
            var adminPassword = adminSection["Password"];

            if (!await AccountExists(adminEmail, userManager))
            {
                var adminUser = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FirstName = "Restaurant",
                    LastName = "Admin"
                };

                await userManager.CreateAsync(adminUser, adminPassword);

                var isAdminClaim = new Claim(AuthConstants.ClaimTypes.IsAdmin, true.ToString());

                await userManager.AddClaimAsync(adminUser, isAdminClaim);
            }
        }

        private static async Task<bool> AccountExists(string email, UserManager<User> userManager) =>
            await userManager.FindByEmailAsync(email) != null;
    }
}