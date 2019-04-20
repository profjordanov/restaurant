using Restaurant.Core._Base;
using Restaurant.Core.MealContext.HttpRequests;

namespace Restaurant.Core.MealContext.Commands
{
    public class RegisterMeal : RegisterMealRequest, ICommand
    {
        public RegisterMeal(string name, decimal price, string typeId, string restaurantId, string userId)
        {
            Name = name;
            Price = price;
            TypeId = typeId;
            RestaurantId = restaurantId;
            UserId = userId;
        }

        public string UserId { get; set; }
    }
}