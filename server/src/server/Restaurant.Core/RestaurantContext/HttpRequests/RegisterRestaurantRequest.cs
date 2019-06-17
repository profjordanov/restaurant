using System;

namespace Restaurant.Core.RestaurantContext.HttpRequests
{
    public class RegisterRestaurantRequest
    {
        public string Name { get; set; }

        public Guid TownId { get; set; }
    }
}