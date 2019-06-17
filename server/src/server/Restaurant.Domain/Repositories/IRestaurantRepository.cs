using System;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IRestaurantRepository
    {
        Task<Domain.Entities.Restaurant> GetByIdAsync(string id);

        Task<Domain.Entities.Restaurant> GetByNameAndTownIdAsync(string name, Guid townId);

        Task<Domain.Entities.Restaurant> SaveAsync(Domain.Entities.Restaurant restaurant);
    }
}