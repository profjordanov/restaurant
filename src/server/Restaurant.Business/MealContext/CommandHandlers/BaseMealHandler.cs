using AutoMapper;
using FluentValidation;
using Marten;
using Restaurant.Business._Base;
using Restaurant.Core._Base;
using Restaurant.Domain.Events._Base;
using Restaurant.Domain.Repositories;

namespace Restaurant.Business.MealContext.CommandHandlers
{
    public abstract class BaseMealHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        protected BaseMealHandler(
            IValidator<TCommand> validator, 
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            IMealRepository mealRepository,
            IRestaurantRepository restaurantRepository,
            IMealTypeRepository mealTypeRepository)
            : base(validator, documentSession, eventBus, mapper)
        {
            MealRepository = mealRepository;
            RestaurantRepository = restaurantRepository;
            MealTypeRepository = mealTypeRepository;
        }

        protected IMealRepository MealRepository { get; }
        protected IRestaurantRepository RestaurantRepository { get; }
        protected IMealTypeRepository MealTypeRepository { get; }
    }
}