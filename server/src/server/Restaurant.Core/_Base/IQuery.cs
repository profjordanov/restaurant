using MediatR;
using Optional;
using Restaurant.Domain;

namespace Restaurant.Core._Base
{
    public interface IQuery<TResponse> : IRequest<Option<TResponse, Error>>
    {
        
    }
}