using Restaurant.Domain.Connectors;
using Restaurant.Domain.Extensions;
using Restaurant.Domain.Readers.Order;
using Restaurant.Domain.SQL;
using Restaurant.Domain.Views.Meal;
using Restaurant.Domain.Views.Order;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Readers.Order
{
    public class PendingOrdersSqlReader : IPendingOrdersReader
    {
        private readonly IQueryDbConnector _queryDbConnector;

        public PendingOrdersSqlReader(IQueryDbConnector queryDbConnector)
        {
            _queryDbConnector = queryDbConnector;
        }

        public async Task<IEnumerable<PendingOrderView>> PendingOrdersAsync(
            Guid userId,
            int limit,
            int startPage,
            CancellationToken cancellationToken)
        {
            var result = await _queryDbConnector.FetchAsync<List<PendingOrderView>>(
                OrderQueryRepository.PendingOrdersByUserIdWithPagingQuery,
                (reader, orders) => orders.Add(ParseReaderResult(reader)),
                new Dictionary<string, object>
                {
                    { "@UserID", userId },
                    { "@LimitCount", limit },
                    { "@OffsetCount", startPage * limit }
                },
                cancellationToken);

            return result;
        }

        private static PendingOrderView ParseReaderResult(DbDataReader reader) =>
            new PendingOrderView
            {
                Id = reader.SafeGetGuid("Id"),
                CreatedOn = CustomParser.ParseDateTime(reader["CreatedOn"]),
                Quantity = CustomParser.ParseInteger(reader["Quantity"]),
                Meal = new MealView
                {
                    Id = reader.SafeGetGuid("MealId"),
                    Name = reader["MealName"].ToString(),
                    Price = CustomParser.ParseDecimal(reader["MealPrice"])
                }
            };
    }
}