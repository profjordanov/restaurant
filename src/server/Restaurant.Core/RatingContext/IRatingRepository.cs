using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Core.RatingContext
{
    public interface IRatingRepository
    {
        Task<Rating> GetByUserIdAndRestaurantIdAsync(string userId, string restaurantId);

        Task<Rating> UpdateAsync(Rating rating);

        Task<Rating> SaveAsync(Rating rating);
    }
}