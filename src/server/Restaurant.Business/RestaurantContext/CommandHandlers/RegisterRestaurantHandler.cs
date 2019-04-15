using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Http;
using Optional;
using Optional.Async;
using Restaurant.Core.RestaurantContext.Commands;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Events._Base;
using Restaurant.Persistence.EntityFramework;

namespace Restaurant.Business.RestaurantContext.CommandHandlers
{
    public class RegisterRestaurantHandler : BaseRestaurantHandler<RegisterRestaurant>
    {
        public RegisterRestaurantHandler(
            IValidator<RegisterRestaurant> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper)
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(RegisterRestaurant command)
        {
            throw new System.NotImplementedException();
        }

        private Task<Option<Domain.Entities.Restaurant, Error>> RestaurantWithCurrentNameAndTownShouldNotExist(
            string name,
            string townId) =>
            DbContext
                .Restaurants
                .FirstOrDefaultAsync(restaurant => restaurant.Name == name &&
                                                   restaurant.TownId.ToString() == townId)
                .SomeWhenAsync(async restaurant => restaurant == null, Error.Conflict($"Restaurant {name} already exists."))
                .MapAsync(async _ => new Domain.Entities.Restaurant());

        private Task<Option<Town, Error>> TownWithCurrentIdShouldExist(string townId) =>
            DbContext
                .Towns
                .SingleOrDefaultAsync(town => town.Id.ToString() == townId)
                .SomeNotNull(Error.Validation($"No town with id {townId} was found."));

        private async Task<Option<Domain.Entities.Restaurant, Error>> PersistRestaurantAsync(RegisterRestaurant command)
        {
            try
            {
                var entry = Mapper.Map<Domain.Entities.Restaurant>(command);
                await DbContext.Restaurants.AddAsync(entry);
                await DbContext.SaveChangesAsync();
                return entry.Some<Domain.Entities.Restaurant, Error>();
            }
            catch (Exception ex)
            {
                Debug.Fail($"Entity Framework Exception: {ex.Message} .");
                return Option.None<Domain.Entities.Restaurant, Error>(
                    Error.Critical("Un error occured while persisting the restaurant data to tha base!"));
            }
        }
    }
}