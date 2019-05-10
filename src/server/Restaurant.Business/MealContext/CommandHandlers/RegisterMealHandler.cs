using AutoMapper;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using Restaurant.Core.MealContext.Commands;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Events._Base;
using Restaurant.Domain.Repositories;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Restaurant.Business.MealContext.CommandHandlers
{
    public class RegisterMealHandler : BaseMealHandler<RegisterMeal>
    {
        public RegisterMealHandler(
            IValidator<RegisterMeal> validator,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            IMealRepository mealRepository,
            IRestaurantRepository restaurantRepository,
            IMealTypeRepository mealTypeRepository)
            : base(validator, documentSession, eventBus, mapper, mealRepository, restaurantRepository, mealTypeRepository)
        {
        }

        public override Task<Option<Unit, Error>> Handle(RegisterMeal command) =>
            EnsureRestaurantExistsAsync(command.RestaurantId).FlatMapAsync(restaurant =>
            RestaurantOwnerShouldRegisterMeal(restaurant, command.UserId).FlatMapAsync(_ =>
            ValidateMealTypeAsync(command.TypeId).FlatMapAsync(__ =>
            PersistMealAsync(command).MapAsync(meal =>
            PublishEventsAsync(meal.Id, meal.RegisterMeal())))));

        private Task<Option<Domain.Entities.Restaurant, Error>> EnsureRestaurantExistsAsync(
            string restaurantId) =>
            RestaurantRepository
                .GetByIdAsync(restaurantId)
                .SomeNotNull(Error.NotFound($"Restaurant with ID `{restaurantId}` does not exists!"));

        private static Option<Domain.Entities.Restaurant, Error> RestaurantOwnerShouldRegisterMeal(
            Domain.Entities.Restaurant restaurant,
            Guid userId) =>
            restaurant.NoneWhen(r => r.OwnerId != userId, Error.Unauthorized("Current user is not the restaurant owner!"));

        private Task<Option<MealType, Error>> ValidateMealTypeAsync(string typeId) =>
            MealTypeRepository
                .GetByIdAsync(typeId)
                .SomeNotNull(Error.Validation($"Invalid meal type ID: {typeId}."));

        private async Task<Option<Meal, Error>> PersistMealAsync(RegisterMeal command)
        {
            try
            {
                var entry = Mapper.Map<Meal>(command);
                await MealRepository.SaveAsync(entry);
                return entry.Some<Meal, Error>();
            }
            catch (Exception e)
            {
                Debug.Fail($"Something fail: {e.Message}");
                return Option.None<Meal, Error>(
                    Error.Critical("Critical error occured while persisting the meal in the database!"));
            }
        }
    }
}