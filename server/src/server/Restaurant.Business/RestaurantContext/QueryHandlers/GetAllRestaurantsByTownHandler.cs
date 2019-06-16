using Optional;
using Restaurant.Core._Base;
using Restaurant.Core.RestaurantContext.Queries;
using Restaurant.Domain;
using Restaurant.Domain.Connectors;
using Restaurant.Domain.Extensions;
using Restaurant.Domain.SQL;
using Restaurant.Domain.Views.Restaurant;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Business.RestaurantContext.QueryHandlers
{
    public class GetAllRestaurantsByTownHandler 
        : IQueryHandler<GetAllRestaurantsByTown, IList<RestaurantWithAvrgRatingView>>
    {
        private readonly IQueryDbConnector _queryDbConnector;

        public GetAllRestaurantsByTownHandler(IQueryDbConnector queryDbConnector)
        {
            _queryDbConnector = queryDbConnector;
        }

        public async Task<Option<IList<RestaurantWithAvrgRatingView>, Error>> Handle(GetAllRestaurantsByTown request, CancellationToken cancellationToken)
        {
            var result = await _queryDbConnector.FetchAsync<List<RestaurantWithAvrgRatingView>>(
                sql: RestaurantQueryRepository.RestaurantsByTownQuery,
                mapping: (reader, restaurants) => restaurants.Add(reader.Get<RestaurantWithAvrgRatingView>()),
                parameters: new Dictionary<string, object>
                {
                    { "@TownID", request.TownId }
                }, 
                cancellationToken: cancellationToken);

            return result.Some<IList<RestaurantWithAvrgRatingView>, Error>();
        }

        [Obsolete]
        private static RestaurantWithAvrgRatingView ParseReaderResult(DbDataReader reader) =>
            new RestaurantWithAvrgRatingView
            {
                Id = reader.SafeGetGuid("Id"),
                Name = reader["Name"].ToString(),
                AverageRating = CustomParser.ParseDecimal(reader["AverageRating"]),
                TownId = reader.SafeGetGuid("TownId")
            };
    }
}