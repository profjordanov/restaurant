using AutoMapper;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using Restaurant.Core.RestaurantContext.Commands;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Events._Base;
using Restaurant.Domain.Repositories;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Restaurant.Business.RestaurantContext.CommandHandlers
{
    public class RegisterRestaurantHandler : BaseRestaurantHandler<RegisterRestaurant>
    {
        private readonly ITownRepository _townRepository;

        public RegisterRestaurantHandler(
            IValidator<RegisterRestaurant> validator,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            IRestaurantRepository restaurantRepository,
            ITownRepository townRepository)
            : base(validator, documentSession, eventBus, mapper, restaurantRepository)
        {
            _townRepository = townRepository;
        }

        public override Task<Option<Unit, Error>> Handle(RegisterRestaurant command) =>
            RestaurantWithCurrentNameAndTownShouldNotExist(command.Name, command.TownId)
                .FlatMapAsync(restaurant =>
            TownWithCurrentIdShouldExist(command.TownId)).FlatMapAsync(_ =>
            PersistRestaurantAsync(command)).MapAsync(restaurant =>
            PublishEventsAsync(restaurant.Id, restaurant.RegisterRestaurant()));

        private async Task<Option<Domain.Entities.Restaurant, Error>> RestaurantWithCurrentNameAndTownShouldNotExist(
            string name,
            string townId) 
        {
            var result = await RestaurantRepository
                .GetByNameAndTownIdAsync(name, townId);

            return result
                .SomeWhen(r => r == null, Error.Conflict($"Restaurant `{name}` already exists."))
                .Map(_ => new Domain.Entities.Restaurant());
        }

        private Task<Option<Town, Error>> TownWithCurrentIdShouldExist(string townId) =>
            _townRepository
                .GetByIdAsync(townId)
                .SomeNotNull(Error.Validation($"No town with ID {townId} was found."));

        private async Task<Option<Domain.Entities.Restaurant, Error>> PersistRestaurantAsync(RegisterRestaurant command)
        {
            try
            {
                var entry = Mapper.Map<Domain.Entities.Restaurant>(command);
                return (await RestaurantRepository.SaveAsync(entry))
                    .Some<Domain.Entities.Restaurant, Error>();
            }
            catch (Exception ex)
            {
                Debug.Fail($"An exception occurred: {ex.Message}.\nInnerException:{ex.InnerException}");
                return Option.None<Domain.Entities.Restaurant, Error>(
                    Error.Critical("Un error occured while persisting the restaurant data to tha base!"));
            }
        }
    }
}