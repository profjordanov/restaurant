using Restaurant.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IRatingRepository
    {
        Task<Rating> GetByUserIdAndRestaurantIdAsync(Guid userId, string restaurantId);

        Task<Rating> UpdateAsync(Rating rating);

        Task<Rating> SaveAsync(Rating rating);
    }
}