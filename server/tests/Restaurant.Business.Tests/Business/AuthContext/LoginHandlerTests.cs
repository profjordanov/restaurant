using Restaurant.Business.Tests.Customizations;
using Restaurant.Business.Tests.Extensions;
using Restaurant.Core.AuthContext.Commands;
using Restaurant.Domain.Enumerations;
using Shouldly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.Business.Tests.Business.AuthContext
{
    public class LoginHandlerTests : ResetDatabaseLifetime
    {
        private const string RegisterUserCommandEmail = "test@email.com";
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
            registerUserCommand.Email = RegisterUserCommandEmail;
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

        [Theory]
        [CustomizedAutoData]
        public async Task CannotLoginWithInvalidCredentials(Register registerUserCommand)
        {
            // Arrange
            registerUserCommand.Email = RegisterUserCommandEmail;
            await _fixture.SendAsync(registerUserCommand);

            var loginCommand = new Login
            {
                Email = registerUserCommand.Email,
                Password = registerUserCommand.Password + "123" // Must be different
            };

            // Act
            var result = await _fixture.SendAsync(loginCommand);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Unauthorized);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotLoginWithInvalidEmail(Login command)
        {
            // Arrange
            command.Email = "invalid-email";

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotLoginWithNonExistingEmail(Login command)
        {
            // Arrange
            // We are not registering any users so the email is definitely not going to be found

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
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