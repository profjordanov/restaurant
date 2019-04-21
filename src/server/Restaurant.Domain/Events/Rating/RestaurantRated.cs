using Restaurant.Domain.Events._Base;
using System;

namespace Restaurant.Domain.Events.Rating
{
    public class RestaurantRated : IEvent
    {
        public Guid RateId { get; set; }

        public Guid RestaurantId { get; set; }

        public string UserId { get; set; }

        public int Stars { get; set; }
    }
}