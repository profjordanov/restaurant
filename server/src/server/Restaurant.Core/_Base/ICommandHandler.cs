using MediatR;
using Optional;
using Restaurant.Domain;

namespace Restaurant.Core._Base
{
    public interface ICommandHandler<in TCommand> :
        IRequestHandler<TCommand, Option<Unit, Error>>
        where TCommand : ICommand
    {
    }

    public interface ICommandHandler<in TCommand, TResult> :
        IRequestHandler<TCommand, Option<TResult, Error>>
        where TCommand : ICommand<TResult>
    {
    }
}