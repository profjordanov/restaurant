using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Persistence.EntityFramework;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Repositories
{
    public class MealRepository : IMealRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MealRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Meal> GetByIdAsync(string mealId) =>
            _dbContext
                .Meals
                .AsNoTracking()
                .SingleOrDefaultAsync(meal => meal.Id.ToString() == mealId);

        public async Task<Meal> SaveAsync(Meal meal)
        {
            await _dbContext.Meals.AddAsync(meal);
            await _dbContext.SaveChangesAsync();
            return meal;
        }
    }
}