using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface ITownRepository
    {
        Task<Town> GetByIdAsync(string townId);
    }
}