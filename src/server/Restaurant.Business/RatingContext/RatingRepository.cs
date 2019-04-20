using Microsoft.EntityFrameworkCore;
using Restaurant.Core.RatingContext;
using Restaurant.Domain.Entities;
using Restaurant.Persistence.EntityFramework;
using System.Threading.Tasks;

namespace Restaurant.Business.RatingContext
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RatingRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public Task<Rating> GetByUserIdAndRestaurantIdAsync(string userId, string restaurantId) =>
            _dbContext
                .Ratings
                .Include(rating => rating.Restaurant)
                .Include(rating => rating.User)
                .FirstOrDefaultAsync(rating => rating.UserId == userId &&
                                               rating.RestaurantId.ToString() == restaurantId);

        public async Task<Rating> UpdateAsync(Rating rating)
        {
            _dbContext
                .Ratings
                .Update(rating);

            await _dbContext.SaveChangesAsync();

            return rating;
        }

        public async Task<Rating> SaveAsync(Rating rating)
        {
            await _dbContext
                .Ratings
                .AddAsync(rating);

            await _dbContext.SaveChangesAsync();

            return rating;
        }
    }
}