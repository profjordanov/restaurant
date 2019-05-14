using Restaurant.Core._Base;
using Restaurant.Core.RestaurantContext.HttpRequests;
using System;

namespace Restaurant.Core.RestaurantContext.Commands
{
    public class RegisterRestaurant : RegisterRestaurantRequest, ICommand
    {
        public RegisterRestaurant()
        {
        }

        public RegisterRestaurant(string name, string townId, Guid ownerId)
        {
            Name = name;
            TownId = townId;
            OwnerId = ownerId;
        }

        public Guid OwnerId { get; set; }
    }
}