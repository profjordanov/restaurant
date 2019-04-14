using FluentValidation;

namespace Restaurant.Core.RestaurantContext.Commands
{
    public class RegisterRestaurantValidator : AbstractValidator<RegisterRestaurant>
    {
        public RegisterRestaurantValidator()
        {
            RuleFor(restaurant => restaurant.Name).NotNull();
            RuleFor(restaurant => restaurant.Name).NotEmpty();

            RuleFor(restaurant => restaurant.TownId).GreaterThan(0);
            RuleFor(restaurant => restaurant.TownId).NotEmpty();
        }
    }
}