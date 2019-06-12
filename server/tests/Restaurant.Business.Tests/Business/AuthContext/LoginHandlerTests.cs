using Restaurant.Business.Tests.Customizations;
using Restaurant.Core.AuthContext.Commands;
using Shouldly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.Business.Tests.Business.AuthContext
{
    public class LoginHandlerTests : ResetDatabaseLifetime
    {
        private readonly AppFixture _fixture;

        public LoginHandlerTests()
        {
            _fixture = new AppFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanLogin(Register registerUserCommand)
        {
            // Arrange
            registerUserCommand.Email = "test@email.com";
            await _fixture.SendAsync(registerUserCommand);

            var loginCommand = new Login
            {
                Email = registerUserCommand.Email,
                Password = registerUserCommand.Password
            };

            // Act
            var result = await _fixture.SendAsync(loginCommand);

            // Assert
            // TODO: Validate subject, claims, etc.
            result.Exists(jwt => IsValidJwtString(jwt.TokenString)).ShouldBeTrue();
        }

        private static bool IsValidJwtString(string tokenString)
        {
            try
            {
                var decodedJwt = new JwtSecurityToken(tokenString);

                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}