using MediatR;
using Optional;
using Restaurant.Domain;

namespace Restaurant.Core._Base
{
    public interface IQueryHandler<in TQuery, TResponse> :
        IRequestHandler<TQuery, Option<TResponse, Error>>
        where TQuery : IQuery<TResponse>
    {
    }
}