using Restaurant.Domain._Base;
using System;
using System.Collections.Generic;
using Restaurant.Domain.Events.Restaurant;

namespace Restaurant.Domain.Entities
{
    public class Restaurant : IAggregate
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid TownId { get; set; }
        public virtual Town Town { get; set; }

        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        public virtual ICollection<Meal> Meals { get; set; } = new HashSet<Meal>();

        public RestaurantRegistered RegisterRestaurant() =>
            new RestaurantRegistered
            {
                RestaurantId = Id,
                Name = Name,
                TownId = TownId,
                OwnerId = OwnerId,
                Date = DateTime.UtcNow
            };
    }
}