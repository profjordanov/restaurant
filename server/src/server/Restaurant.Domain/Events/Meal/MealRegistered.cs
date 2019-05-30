using Restaurant.Domain.Events._Base;
using System;

namespace Restaurant.Domain.Events.Meal
{
    public class MealRegistered : IEvent
    {
        public Guid MealId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public Guid RestaurantId { get; set; }

        public Guid TypeId { get; set; }
    }
}