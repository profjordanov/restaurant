using MediatR;
using Optional;
using Restaurant.Domain;

namespace Restaurant.Core._Base
{
    public interface ICommand : IRequest<Option<Unit, Error>>
    {
    }

    public interface ICommand<TResult> : IRequest<Option<TResult, Error>>
    {
    }
}
