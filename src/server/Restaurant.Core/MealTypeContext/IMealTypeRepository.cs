using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Core.MealTypeContext
{
    public interface IMealTypeRepository
    {
        Task<MealType> GetByIdAsync(string typeId);
    }
}