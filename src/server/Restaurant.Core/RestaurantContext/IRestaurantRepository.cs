using System.Threading.Tasks;

namespace Restaurant.Core.RestaurantContext
{
    public interface IRestaurantRepository
    {
        Task<Domain.Entities.Restaurant> GetByNameAndTownIdAsync(string name, string townId);
        Task<Domain.Entities.Restaurant> AddAsync(Domain.Entities.Restaurant restaurant);
    }
}