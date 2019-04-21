using System.Threading.Tasks;
using Restaurant.Core.OrderContext;
using Restaurant.Domain.Entities;
using Restaurant.Persistence.EntityFramework;

namespace Restaurant.Business.OrderContext
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> SaveAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }
    }
}