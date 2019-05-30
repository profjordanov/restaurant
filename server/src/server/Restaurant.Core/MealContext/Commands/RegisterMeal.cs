using Restaurant.Core._Base;
using Restaurant.Core.MealContext.HttpRequests;
using System;

namespace Restaurant.Core.MealContext.Commands
{
    public class RegisterMeal : RegisterMealRequest, ICommand
    {
        public RegisterMeal(string name, decimal price, string typeId, string restaurantId, Guid userId)
        {
            Name = name;
            Price = price;
            TypeId = typeId;
            RestaurantId = restaurantId;
            UserId = userId;
        }

        public Guid UserId { get; set; }
    }
}