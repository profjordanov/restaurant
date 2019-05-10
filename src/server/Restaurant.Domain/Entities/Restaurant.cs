using Restaurant.Domain._Base;
using Restaurant.Domain.Events.Restaurant;
using System;
using System.Collections.Generic;

namespace Restaurant.Domain.Entities
{
    public class Restaurant : IAggregate
    {
        // Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid TownId { get; set; }

        public Guid OwnerId { get; set; }

        // Relations
        public virtual User Owner { get; set; }

        public virtual Town Town { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        public virtual ICollection<Meal> Meals { get; set; } = new HashSet<Meal>();

        // Events
        public RestaurantRegistered RegisterRestaurant() =>
            new RestaurantRegistered
            {
                RestaurantId = Id,
                Name = Name,
                TownId = TownId,
                OwnerId = OwnerId,
                Date = DateTime.UtcNow
            };

        public void Apply(RestaurantRegistered @event)
        {
            Id = @event.RestaurantId;
            Name = @event.Name;
            TownId = @event.TownId;
            OwnerId = @event.OwnerId;
        }

    }
}