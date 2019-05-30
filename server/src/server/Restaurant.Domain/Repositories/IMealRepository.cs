using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IMealRepository
    {
        Task<Meal> GetByIdAsync(string mealId);

        Task<Meal> SaveAsync(Meal meal);
    }
}