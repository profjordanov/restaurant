using System;
using Restaurant.Domain.Enumerations;

namespace Restaurant.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid MealId { get; set; }
        public virtual Meal Meal { get; set; }

        public int Quantity { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}