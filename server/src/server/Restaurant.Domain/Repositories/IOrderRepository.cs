using Restaurant.Domain.Entities;
using Restaurant.Domain.Specifications._Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetList(
            Specification<Order> specification,
            Guid userId,
            CancellationToken cancellationToken,
            int page = 0,
            int pageSize = 20);

        Task<Order> SaveAsync(Order order);
    }
}