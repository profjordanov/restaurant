using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Domain.Entities
{
    public class Meal
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        public int TypeId { get; set; }
        public virtual MealType Type { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}