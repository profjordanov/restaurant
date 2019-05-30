using FluentValidation;
using Restaurant.Core.RestaurantContext.Commands;

namespace Restaurant.Core.RatingContext.Commands
{
    public class RateRestaurantValidator : AbstractValidator<RateRestaurant>
    {
        public RateRestaurantValidator()
        {
            RuleFor(rate => rate.Stars).GreaterThanOrEqualTo(1);
            RuleFor(rate => rate.Stars).LessThanOrEqualTo(10);
        }
    }
}