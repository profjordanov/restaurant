using System;
using Optional;
using Restaurant.Business.Extensions;
using Restaurant.Core._Base;
using Restaurant.Core.OrderContext.Queries;
using Restaurant.Domain;
using Restaurant.Domain.Connectors;
using Restaurant.Domain.SQL;
using Restaurant.Domain.Views.Meal;
using Restaurant.Domain.Views.Order;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Optional.Async;
using Restaurant.Business.OrderContext.Specifications;
using Restaurant.Domain.Repositories;

namespace Restaurant.Business.OrderContext.QueryHandlers
{
    public class PendingOrdersByUserWithPagingHandler
        : IQueryHandler<GetPendingOrdersByUser, IList<PendingOrderView>>
    {
        private readonly IMapper _mapper;
        private readonly IQueryDbConnector _queryDbConnector;
        private readonly IOrderRepository _repository;

        public PendingOrdersByUserWithPagingHandler(
            IQueryDbConnector queryDbConnector,
            IOrderRepository repository,
            IMapper mapper)
        {
            _queryDbConnector = queryDbConnector;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Option<IList<PendingOrderView>, Error>> Handle(
            GetPendingOrdersByUser request,
            CancellationToken cancellationToken)
        {
            var collection = await _repository
                .GetList(
                    specification: new PendingOrdersSpecification(),
                    userId: request.UserId,
                    cancellationToken: cancellationToken,
                    page: request.StartPage,
                    pageSize: request.Limit);

            return _mapper.Map<IList<PendingOrderView>>(collection)
                .SomeNotNull(Error.NotFound("Something"));
        }

        //public async Task<Option<IList<PendingOrderView>, Error>> Handle(
        //    GetPendingOrdersByUser request,
        //    CancellationToken cancellationToken)
        //{
        //    var result = await _queryDbConnector.FetchAsync<List<PendingOrderView>>(
        //        sql: OrderQueryRepository.PendingOrdersByUserIdWithPagingQuery,
        //        mapping: (reader, orders) => orders.Add(ParseReaderResult(reader)),
        //        parameters: new Dictionary<string, object>
        //        {
        //            { "@UserID", request.UserId },
        //            { "@LimitCount", request.Limit },
        //            { "@OffsetCount", (request.StartPage - 1) * request.Limit }
        //        },
        //        cancellationToken: cancellationToken);

        //    return result.Some<IList<PendingOrderView>, Error>();
        //}

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