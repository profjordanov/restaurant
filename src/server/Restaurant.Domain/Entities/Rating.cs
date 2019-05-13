using Restaurant.Domain._Base;
using Restaurant.Domain.Events.Rating;
using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Domain.Entities
{
    public class Rating : IAggregate
    {
        // Properties
        public Guid Id { get; set; }

        [Range(0, 10)]
        public int Stars { get; set; }

        public Guid UserId { get; set; }

        public Guid RestaurantId { get; set; }

        // Relations
        public virtual Restaurant Restaurant { get; set; }
        public virtual User User { get; set; }

        // Events
        public RestaurantRated RateRestaurant() =>
            new RestaurantRated
            {
                RateId = Id,
                RestaurantId = RestaurantId,
                UserId = UserId,
                Stars = Stars
            };

        public void Apply(RestaurantRated @event)
        {
            Id = @event.RateId;
            RestaurantId = @event.RestaurantId;
            UserId = @event.UserId;
            Stars = @event.Stars;
        }
    }
}