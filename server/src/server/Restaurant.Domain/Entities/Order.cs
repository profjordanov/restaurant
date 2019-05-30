using Restaurant.Domain._Base;
using Restaurant.Domain.Enumerations;
using Restaurant.Domain.Events.Order;
using System;

namespace Restaurant.Domain.Entities
{
    public class Order : IAggregate
    {
        // PROPERTIES
        public Guid Id { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid MealId { get; set; }

        public int Quantity { get; set; }

        public Guid UserId { get; set; }

        // RELATIONS 
        public virtual Meal Meal { get; set; }

        public virtual User User { get; set; }

        // EVENTS
        public OrderMade MakeNewOrder(Guid restaurantId, Guid mealTypeId) =>
            new OrderMade
            {
                OrderId = Id,
                MealId = MealId,
                Quantity = Quantity,
                CreatedOn = CreatedOn,
                UserId = UserId,
                OrderStatus = OrderStatus,
                RestaurantId = restaurantId,
                MealTypeId = mealTypeId
            };

        public void Apply(OrderMade @event)
        {
            Id = @event.OrderId;
            CreatedOn = @event.CreatedOn;
            MealId = @event.MealId;
            UserId = @event.UserId;
            Quantity = @event.Quantity;
            OrderStatus = @event.OrderStatus;
        }
    }
}