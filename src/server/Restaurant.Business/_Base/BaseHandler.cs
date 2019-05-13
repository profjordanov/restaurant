using AutoMapper;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using Restaurant.Core._Base;
using Restaurant.Domain;
using Restaurant.Domain.Events._Base;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Business._Base
{
    public abstract class BaseHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        protected BaseHandler(
            IValidator<TCommand> validator,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
        {
            Validator = validator ??
                throw new InvalidOperationException(
                    "Tried to instantiate a command handler without a validator." +
                    "Did you forget to add one?");

            Session = documentSession;
            EventBus = eventBus;
            Mapper = mapper;
        }

        protected IEventBus EventBus { get; }
        protected IMapper Mapper { get; }
        protected IDocumentSession Session { get; }
        protected IValidator<TCommand> Validator { get; }

        public Task<Option<Unit, Error>> Handle(TCommand command, CancellationToken cancellationToken) =>
            ValidateCommand(command).FlatMapAsync(Handle);

        public abstract Task<Option<Unit, Error>> Handle(TCommand command);

        protected async Task<Unit> PublishEventsAsync(Guid streamId, params IEvent[] events)
        {
            Session.Events.Append(streamId, events);
            await Session.SaveChangesAsync();
            await EventBus.Publish(events);

            return Unit.Value;
        }

        protected Option<TCommand, Error> ValidateCommand(TCommand command)
        {
            var validationResult = Validator.Validate(command);

            return validationResult
                .SomeWhen(
                    r => r.IsValid,
                    r => Error.Validation(r.Errors.Select(e => e.ErrorMessage)))

                // If the validation result is successful, disregard it and simply return the command
                .Map(_ => command);
        }
    }
}
