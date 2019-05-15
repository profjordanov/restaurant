using Restaurant.Core._Base;
using Restaurant.Core.OrderContext.HttpRequests;
using System;

namespace Restaurant.Core.OrderContext.Commands
{
    public class MakeNewOrder : MakeOrderRequest, ICommand
    {
        public MakeNewOrder(string mealId, int quantity, Guid userId)
        {
            UserId = userId;
            MealId = mealId;
            Quantity = quantity;
        }

        public string MealId { get; set; }

        public Guid UserId { get; set; }
    }
}