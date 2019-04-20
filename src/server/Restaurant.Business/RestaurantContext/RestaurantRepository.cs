using Microsoft.EntityFrameworkCore;
using Restaurant.Core.RestaurantContext;
using Restaurant.Persistence.EntityFramework;
using System.Threading.Tasks;

namespace Restaurant.Business.RestaurantContext
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RestaurantRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public Task<Domain.Entities.Restaurant> GetByNameAndTownIdAsync(string name, string townId) =>
            _dbContext
                .Restaurants
                .FirstOrDefaultAsync(restaurant => restaurant.Name == name &&
                                                   restaurant.TownId.ToString() == townId);

        public async Task<Domain.Entities.Restaurant> AddAsync(Domain.Entities.Restaurant restaurant)
        {
            await _dbContext.AddAsync(restaurant);
            await _dbContext.SaveChangesAsync();
            return restaurant;
        }
    }
}