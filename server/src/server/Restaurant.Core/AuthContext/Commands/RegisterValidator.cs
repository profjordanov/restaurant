using FluentValidation;

namespace Restaurant.Core.AuthContext.Commands
{
    public class RegisterValidator : AbstractValidator<Register>
    {
        public RegisterValidator()
        {
            RuleFor(c => c.Email).NotNull();
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.FirstName).NotNull();
            RuleFor(c => c.LastName).NotNull();
            RuleFor(c => c.Password).NotNull();
        }
    }
}