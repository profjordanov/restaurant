using Restaurant.Domain.Readers.Order;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.Specifications.Order;
using Restaurant.Domain.Views.Meal;
using Restaurant.Domain.Views.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Readers.Order
{
    public class PendingOrdersEfReader : IPendingOrdersReader
    {
        private readonly IOrderRepository _orderRepository;

        public PendingOrdersEfReader(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<PendingOrderView>> PendingOrdersAsync(
            Guid userId, int limit, int startPage, CancellationToken cancellationToken)
        {
            var result = await _orderRepository.GetList(
                new PendingOrdersSpecification(), userId, cancellationToken, startPage, limit);

            return result.Select(order => new PendingOrderView
            {
                Id = order.Id,
                Quantity = order.Quantity,
                CreatedOn = order.CreatedOn,
                Meal = new MealView
                {
                    Id = order.MealId,
                    Name = order.Meal.Name,
                    Price = order.Meal.Price
                }
            });
        }
    }
}