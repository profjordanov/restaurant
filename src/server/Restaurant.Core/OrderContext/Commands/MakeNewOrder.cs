using Restaurant.Core._Base;
using Restaurant.Core.OrderContext.HttpRequests;

namespace Restaurant.Core.OrderContext.Commands
{
    public class MakeNewOrder : MakeOrderRequest, ICommand
    {
        public MakeNewOrder(string mealId, int quantity, string userId)
        {
            UserId = userId;
            MealId = mealId;
            Quantity = quantity;
        }

        public string MealId { get; set; }

        public string UserId { get; set; }
    }
}