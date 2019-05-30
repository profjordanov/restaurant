using AutoMapper;
using FluentValidation;
using Marten;
using Restaurant.Business._Base;
using Restaurant.Core._Base;
using Restaurant.Domain.Events._Base;
using Restaurant.Domain.Repositories;

namespace Restaurant.Business.RestaurantContext.CommandHandlers
{
    public abstract class BaseRestaurantHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        protected BaseRestaurantHandler(
            IValidator<TCommand> validator,  
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper, 
            IRestaurantRepository restaurantRepository) 
            : base(validator, documentSession, eventBus, mapper)
        {
            RestaurantRepository = restaurantRepository;
        }

        protected IRestaurantRepository RestaurantRepository { get; }
    }
}