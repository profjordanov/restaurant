using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Core.TownContext
{
    public interface ITownRepository
    {
        Task<Town> GetByIdAsync(string townId);
    }
}