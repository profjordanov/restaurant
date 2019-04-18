using Restaurant.Core._Base;
using Restaurant.Core.RestaurantContext.HttpRequests;

namespace Restaurant.Core.RestaurantContext.Commands
{
    public class RegisterRestaurant : RegisterRestaurantRequest, ICommand
    {
        public RegisterRestaurant()
        {
        }

        public RegisterRestaurant(string name, string townId, string ownerId)
        {
            Name = name;
            TownId = townId;
            OwnerId = ownerId;
        }

        public string OwnerId { get; set; }
    }
}