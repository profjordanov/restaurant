using System.Collections.Generic;

namespace Restaurant.Domain.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        public virtual ICollection<Meal> Meals { get; set; } = new HashSet<Meal>();
    }
}