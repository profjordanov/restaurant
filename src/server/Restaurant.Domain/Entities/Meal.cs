using Restaurant.Domain._Base;
using Restaurant.Domain.Events.Meal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Domain.Entities
{
    public class Meal : IAggregate
    {
        // Properties
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public Guid RestaurantId { get; set; }

        public Guid TypeId { get; set; }

        // Relations
        public virtual Restaurant Restaurant { get; set; }

        public virtual MealType Type { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        // Events
        public MealRegistered RegisterMeal() =>
            new MealRegistered
            {
                MealId = Id,
                Name = Name,
                RestaurantId = RestaurantId,
                Price = Price,
                TypeId = TypeId
            };

        public void Apply(MealRegistered @event)
        {
            Id = @event.MealId;
            Name = @event.Name;
            Price = @event.Price;
            RestaurantId = @event.RestaurantId;
            TypeId = @event.TypeId;
        }
    }
}