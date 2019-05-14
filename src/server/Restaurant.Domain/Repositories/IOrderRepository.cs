using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> SaveAsync(Order order);
    }
}