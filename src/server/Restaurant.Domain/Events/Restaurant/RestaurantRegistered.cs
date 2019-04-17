using System;
using Restaurant.Domain.Events._Base;

namespace Restaurant.Domain.Events.Restaurant
{
    public class RestaurantRegistered : IEvent
    {
        public Guid RestaurantId { get; set; }

        public string Name { get; set; }

        public Guid TownId { get; set; }

        public string OwnerId { get; set; }

        public DateTime Date { get; set; }
    }
}