using AutoMapper;
using Optional;
using Restaurant.Core._Base;
using Restaurant.Core.OrderContext.Queries;
using Restaurant.Domain;
using Restaurant.Domain.Connectors;
using Restaurant.Domain.Decorators.Order;
using Restaurant.Domain.Extensions;
using Restaurant.Domain.Readers.Order;
using Restaurant.Domain.Repositories;
using Restaurant.Domain.Specifications.Order;
using Restaurant.Domain.Views.Meal;
using Restaurant.Domain.Views.Order;
using Restaurant.Persistence.Readers.Order;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Business.OrderContext.QueryHandlers
{
    public class PendingOrdersByUserWithPagingHandler
        : IQueryHandler<GetPendingOrdersByUser, IEnumerable<PendingOrderView>>
    {
        private readonly IQueryDbConnector _queryDbConnector;
        private readonly IOrderRepository _repository;

        public PendingOrdersByUserWithPagingHandler(
            IQueryDbConnector queryDbConnector,
            IOrderRepository repository)
        {
            _queryDbConnector = queryDbConnector;
            _repository = repository;
        }

        public async Task<Option<IEnumerable<PendingOrderView>, Error>> Handle(
            GetPendingOrdersByUser request, CancellationToken cancellationToken)
        {
            IPendingOrdersReader dataProvider;
            switch (request.DataProvider)
            {
                case Core.OrderContext.Enums.PendingOrdersDataProvider.CsvProvider:
                    dataProvider = new PendingOrdersCsvReader();
                    break;
                case Core.OrderContext.Enums.PendingOrdersDataProvider.EntityFrameworkProvider:
                    dataProvider = new PendingOrdersEfReader(_repository);
                    break;
                case Core.OrderContext.Enums.PendingOrdersDataProvider.AdoNetProvider:
                    dataProvider = new PendingOrdersSqlReader(_queryDbConnector);
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }

            var cachingReader = new PendingOrdersCachingReader(dataProvider);

            var result = await cachingReader.PendingOrdersAsync(
                request.UserId, request.Limit, request.StartPage, cancellationToken);

            return result.SomeWhen(list => list.Any(), Error.NotFound("There are no pending orders for current user!"));
        }
    }
}