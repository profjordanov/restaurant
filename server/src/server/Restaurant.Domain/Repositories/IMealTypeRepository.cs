using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IMealTypeRepository
    {
        Task<MealType> GetByIdAsync(string typeId);
    }
}