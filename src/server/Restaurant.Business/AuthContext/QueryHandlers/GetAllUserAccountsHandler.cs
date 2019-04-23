using Optional;
using Restaurant.Core._Base;
using Restaurant.Core.AuthContext.Queries;
using Restaurant.Domain;
using Restaurant.Domain.Connectors;
using Restaurant.Domain.Views.Auth;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Restaurant.Business.Extensions;
using Restaurant.Domain.SQL;

namespace Restaurant.Business.AuthContext.QueryHandlers
{
    public class GetAllUserAccountsHandler : IQueryHandler<GetAllUserAccounts, IList<UserView>> 
    {
        private readonly IQueryDbConnector _queryDbConnector;

        public GetAllUserAccountsHandler(IQueryDbConnector queryDbConnector)
        {
            _queryDbConnector = queryDbConnector;
        }

        public async Task<Option<IList<UserView>, Error>> Handle(
            GetAllUserAccounts request,
            CancellationToken cancellationToken)
        {
            var result = await _queryDbConnector.FetchAsync<List<UserView>>(
                sql: UserAccountQueryRepository.UserAccountsQuery,
                mapping: (reader, users) => users.Add(reader.Get<UserView>()));

            return result.Some<IList<UserView>, Error>();
        }
    }
}
