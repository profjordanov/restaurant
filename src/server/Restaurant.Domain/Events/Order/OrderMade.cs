using Restaurant.Domain.Enumerations;
using Restaurant.Domain.Events._Base;
using System;

namespace Restaurant.Domain.Events.Order
{
    public class OrderMade : IEvent
    {
        public Guid OrderId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid MealId { get; set; }

        public Guid MealTypeId { get; set; }

        public int Quantity { get; set; }

        public Guid UserId { get; set; }

        public Guid RestaurantId { get; set; }
    }
}