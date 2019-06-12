using Microsoft.EntityFrameworkCore;
using Restaurant.Business.Tests.Customizations;
using Restaurant.Business.Tests.Extensions;
using Restaurant.Core.AuthContext.Commands;
using Restaurant.Domain.Enumerations;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Restaurant.Business.Tests.Business.AuthContext
{
    public class RegisterHandlerTests : ResetDatabaseLifetime
    {
        private const string RegisterUserCommandEmail = "test@email.com";
        private readonly AppFixture _fixture;

        public RegisterHandlerTests()
        {
            _fixture = new AppFixture();
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CanRegister(Register command)
        {
            // Act
            command.Email = RegisterUserCommandEmail;
            var result = await _fixture.SendAsync(command);

            // Assert
            result.HasValue.ShouldBeTrue();

            var userInDb = await _fixture
                .ExecuteDbContextAsync(context => context.Users.FirstOrDefaultAsync(u => u.Email == command.Email));

            userInDb.Email.ShouldBe(command.Email);
            userInDb.FirstName.ShouldBe(command.FirstName);
            userInDb.LastName.ShouldBe(command.LastName);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotRegisterWithInvalidEmail(Register command)
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
        public async Task CannotRegisterTheSameEmailTwice(Register command)
        {
            // Arrange
            // First register
            command.Email = RegisterUserCommandEmail;
            await _fixture.SendAsync(command);

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Conflict);
        }

        [Theory]
        [CustomizedAutoData]
        public async Task CannotRegisterWithMissingNames(Register command)
        {
            // Arrange
            command.FirstName = null;
            command.LastName = null;

            // Act
            var result = await _fixture.SendAsync(command);

            // Assert
            result.ShouldHaveErrorOfType(ErrorType.Validation);
        }
    }
}