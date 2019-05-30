using Restaurant.Core._Base;
using Restaurant.Core.RatingContext.HttpRequests;
using System;

namespace Restaurant.Core.RatingContext.Commands
{
    public class RateRestaurant : RateRestaurantRequest, ICommand
    {
        public RateRestaurant()
        {           
        }

        public RateRestaurant(int stars, string restaurantId, Guid userId)
        {
            Stars = stars;
            RestaurantId = restaurantId;
            UserId = userId;
        }

        public string RestaurantId { get; set; }

        public Guid UserId { get; set; }
    }
}