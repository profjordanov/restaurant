using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.Specifications._Base;
using Restaurant.Persistence.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Order>> GetList(
            Specification<Order> specification,
            Guid userId,
            CancellationToken cancellationToken,
            int page = 0,
            int pageSize = 20) =>
            _dbContext
                .Orders
                .Where(specification.ToExpression())
                .Where(order => order.UserId == userId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Include(order => order.Meal)
                .ToListAsync(cancellationToken);

        public async Task<Order> SaveAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }
    }
}