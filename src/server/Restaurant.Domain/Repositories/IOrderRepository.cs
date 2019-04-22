using System.Threading.Tasks;
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> SaveAsync(Order order);
    }
}