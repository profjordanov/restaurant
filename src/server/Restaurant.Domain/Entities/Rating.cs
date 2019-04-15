using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Domain.Entities
{
    public class Rating
    {
        public Guid Id { get; set; }

        [Range(0, 10)]
        public int Stars { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public Guid RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}