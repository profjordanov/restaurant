using AutoMapper;
using FluentValidation;
using Marten;
using Restaurant.Business._Base;
using Restaurant.Core._Base;
using Restaurant.Domain.Events._Base;
using Restaurant.Domain.Repositories;

namespace Restaurant.Business.RatingContext.CommandHandlers
{
    public abstract class BaseRatingHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        protected BaseRatingHandler(
            IValidator<TCommand> validator,
            IDocumentSession documentSession,
            IEventBus eventBus, 
            IMapper mapper, 
            IRatingRepository ratingRepository) 
            : base(validator, documentSession, eventBus, mapper)
        {
            RatingRepository = ratingRepository;
        }

        protected IRatingRepository RatingRepository { get; }
    }
}