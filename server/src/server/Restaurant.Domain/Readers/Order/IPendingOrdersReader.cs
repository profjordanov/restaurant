using Restaurant.Domain.Views.Order;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Domain.Readers.Order
{
    public interface IPendingOrdersReader
    {
        Task<IEnumerable<PendingOrderView>> PendingOrdersAsync(
            Guid userId,
            int limit,
            int startPage,
            CancellationToken cancellationToken);
    }
}