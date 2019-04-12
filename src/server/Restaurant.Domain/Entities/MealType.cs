using System.Collections.Generic;

namespace Restaurant.Domain.Entities
{
    public class MealType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Meal> Meals { get; set; } = new HashSet<Meal>();
    }
}