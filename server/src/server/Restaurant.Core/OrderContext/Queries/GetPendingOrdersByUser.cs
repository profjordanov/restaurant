using Restaurant.Core._Base;
using Restaurant.Core.OrderContext.HttpRequests;
using Restaurant.Domain.Views.Order;
using System;
using System.Collections.Generic;

namespace Restaurant.Core.OrderContext.Queries
{
    public class GetPendingOrdersByUser : GetPendingOrdersRequest, IQuery<IList<PendingOrderView>>
    {
        public GetPendingOrdersByUser(Guid userId, GetPendingOrdersRequest request)
        {
            UserId = userId;
            StartPage = request.StartPage;
            Limit = request.Limit;
        }

        public Guid UserId { get; set; }
    }
}