using FluentValidation;

namespace Restaurant.Core.MealContext.Commands
{
    public class RegisterMealValidator : AbstractValidator<RegisterMeal>
    {
        public RegisterMealValidator()
        {
            RuleFor(meal => meal.UserId).NotEmpty();
            RuleFor(meal => meal.Name).NotEmpty();
            RuleFor(meal => meal.Price).NotEmpty();
            RuleFor(meal => meal.TypeId).NotEmpty();
            RuleFor(meal => meal.RestaurantId).NotEmpty();
        }
    }
}