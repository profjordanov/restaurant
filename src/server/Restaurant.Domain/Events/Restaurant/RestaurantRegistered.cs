using System;
using Restaurant.Domain.Events._Base;

namespace Restaurant.Domain.Events.Restaurant
{
    public class RestaurantRegistered : IEvent
    {
        public Guid RestaurantId { get; set; }

        public string Name { get; set; }

        public string Town { get; set; }

        public string Owner { get; set; }

        public DateTime Date { get; set; }
    }
}