using FluentValidation;

namespace Restaurant.Core.AuthContext.Commands
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(c => c.Email).NotNull();
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.Password).NotNull();
        }
    }
}