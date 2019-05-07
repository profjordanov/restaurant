using Restaurant.Core._Base;
using Restaurant.Core.OrderContext.HttpRequests;
using Restaurant.Domain.Views.Order;
using System.Collections.Generic;

namespace Restaurant.Core.OrderContext.Queries
{
    public class GetPendingOrdersByUser : GetPendingOrdersRequest, IQuery<IList<PendingOrderView>>
    {
        public GetPendingOrdersByUser(string userId, int startPage, int limit)
        {
            UserId = userId;
            StartPage = startPage;
            Limit = limit;
        }

        public string UserId { get; set; }
    }
}