using Restaurant.Domain.Events._Base;
using System;

namespace Restaurant.Domain.Events.Restaurant
{
    public class RestaurantRegistered : IEvent
    {
        public Guid RestaurantId { get; set; }

        public string Name { get; set; }

        public Guid TownId { get; set; }

        public Guid OwnerId { get; set; }

        public DateTime Date { get; set; }
    }
}