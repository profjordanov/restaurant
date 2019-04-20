using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Core.MealContext
{
    public interface IMealRepository
    {
        Task<Meal> SaveAsync(Meal meal);
    }
}