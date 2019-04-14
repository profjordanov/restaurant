using Newtonsoft.Json;
using Restaurant.Core._Base;

namespace Restaurant.Core.RestaurantContext.Commands
{
    public class RegisterRestaurant : ICommand
    {
        public string Name { get; set; }

        public int TownId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OwnerId { get; set; }
    }
}