using System.ComponentModel.DataAnnotations;

namespace Restaurant.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }

        [Range(0, 10)]
        public int Stars { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}