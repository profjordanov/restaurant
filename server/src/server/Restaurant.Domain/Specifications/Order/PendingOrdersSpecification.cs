using Restaurant.Domain.Enumerations;
using Restaurant.Domain.Specifications._Base;
using System;
using System.Linq.Expressions;

namespace Restaurant.Domain.Specifications.Order
{
    public sealed class PendingOrdersSpecification : Specification<Entities.Order>
    {
        public override Expression<Func<Entities.Order, bool>> ToExpression()
        {
            return order => order.OrderStatus == OrderStatus.Pending;
        }
    }
}