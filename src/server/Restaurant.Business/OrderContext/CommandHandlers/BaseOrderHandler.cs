using AutoMapper;
using FluentValidation;
using Marten;
using Restaurant.Business._Base;
using Restaurant.Core._Base;
using Restaurant.Domain.Events._Base;
using Restaurant.Domain.Repositories;

namespace Restaurant.Business.OrderContext.CommandHandlers
{
    public abstract class BaseOrderHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        protected BaseOrderHandler(
            IValidator<TCommand> validator,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper, 
            IOrderRepository orderRepository,
            IMealRepository mealRepository) 
            : base(validator, documentSession, eventBus, mapper)
        {
            OrderRepository = orderRepository;
            MealRepository = mealRepository;
        }

        protected IOrderRepository OrderRepository { get; }
        protected IMealRepository MealRepository { get; }
    }
}