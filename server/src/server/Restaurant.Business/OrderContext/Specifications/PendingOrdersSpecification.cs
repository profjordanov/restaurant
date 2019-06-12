using Restaurant.Domain.Entities;
using Restaurant.Domain.Enumerations;
using Restaurant.Domain.SpecificationPattern;
using System;
using System.Linq.Expressions;

namespace Restaurant.Business.OrderContext.Specifications
{
    public sealed class PendingOrdersSpecification : Specification<Order>
    {
        public override Expression<Func<Order, bool>> ToExpression()
        {
            return order => order.OrderStatus == OrderStatus.Pending;
        }
    }
}