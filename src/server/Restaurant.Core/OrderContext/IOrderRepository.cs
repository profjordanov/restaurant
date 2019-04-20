using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Core.OrderContext
{
    public interface IOrderRepository
    {
        Task<Order> SaveAsync(Order order);
    }
}