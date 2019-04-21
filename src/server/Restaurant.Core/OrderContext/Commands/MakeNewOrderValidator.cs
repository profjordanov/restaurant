using FluentValidation;

namespace Restaurant.Core.OrderContext.Commands
{
    public class MakeNewOrderValidator : AbstractValidator<MakeNewOrder>
    {
        public MakeNewOrderValidator()
        {
            RuleFor(order => order.Quantity).GreaterThanOrEqualTo(1);
            RuleFor(order => order.MealId).NotEmpty();
            RuleFor(order => order.UserId).NotEmpty();
        }
    }
}