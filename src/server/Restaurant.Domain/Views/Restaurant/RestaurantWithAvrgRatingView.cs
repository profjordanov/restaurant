using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Views.Restaurant
{
    public class RestaurantWithAvrgRatingView
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal AverageRating { get; set; }

        public Guid TownId { get; set; }
    }
}
