using AutoMapper;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using Restaurant.Core.OrderContext.Commands;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Events._Base;
using Restaurant.Domain.Repositories;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Restaurant.Business.OrderContext.CommandHandlers
{
    public class MakeOrderHandler : BaseOrderHandler<MakeNewOrder>
    {
        public MakeOrderHandler(
            IValidator<MakeNewOrder> validator,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            IOrderRepository orderRepository,
            IMealRepository mealRepository) 
            : base(validator, documentSession, eventBus, mapper, orderRepository, mealRepository)
        {
        }

        public override Task<Option<Unit, Error>> Handle(MakeNewOrder command) =>
            EnsureMealExistsByIdAsync(command.MealId).FlatMapAsync(meal =>
            PersistOrderAsync(command).MapAsync(order =>
            PublishEventsAsync(order.Id, order.MakeNewOrder(meal.RestaurantId, meal.TypeId))));

        private Task<Option<Meal, Error>> EnsureMealExistsByIdAsync(string mealId) =>
            MealRepository
                .GetByIdAsync(mealId)
                .SomeNotNull(Error.NotFound($"Cannot find meal with ID: {mealId}"));

        private async Task<Option<Order, Error>> PersistOrderAsync(MakeNewOrder command)
        {
            var entry = Mapper.Map<Order>(command);
            try
            {
                var result = await OrderRepository.SaveAsync(entry);
                return result.Some<Order, Error>();
            }
            catch (Exception e)
            {
                Debug.Fail("FATAL ERROR: " + e.Message);
                return Option.None<Order, Error>(
                    Error.Critical("Unexpected exception occurred when saving order in the database!"));
            }
        }
    }
}