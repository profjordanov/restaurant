using Restaurant.Core._Base;
using Restaurant.Domain.Views.Restaurant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Core.RestaurantContext.Queries
{
    public class GetAllRestaurantsByTown : IQuery<IList<RestaurantWithAvrgRatingView>>
    {
        [Required]
        public Guid TownId { get; set; }
    }
}