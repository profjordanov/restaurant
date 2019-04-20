using Restaurant.Core.MealContext;
using Restaurant.Domain.Entities;
using Restaurant.Persistence.EntityFramework;
using System.Threading.Tasks;

namespace Restaurant.Business.MealContext
{
    public class MealRepository : IMealRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MealRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Meal> SaveAsync(Meal meal)
        {
            await _dbContext.Meals.AddAsync(meal);
            await _dbContext.SaveChangesAsync();
            return meal;
        }
    }
}