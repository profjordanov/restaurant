using AutoMapper;
using FluentValidation;
using Marten;
using MediatR;
using Optional;
using Optional.Async;
using Restaurant.Core.RatingContext.Commands;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Events._Base;
using Restaurant.Domain.Repositories;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Restaurant.Business.RatingContext.CommandHandlers
{
    public class RateRestaurantHandler : BaseRatingHandler<RateRestaurant>
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RateRestaurantHandler(
            IValidator<RateRestaurant> validator,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            IRatingRepository ratingRepository,
            IRestaurantRepository restaurantRepository)
            : base(validator, documentSession, eventBus, mapper, ratingRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public override Task<Option<Unit, Error>> Handle(RateRestaurant command) =>
            EnsureRestaurantExistsAsync(command.RestaurantId).FlatMapAsync(restaurant =>
            UserShouldNotRateOwnRestaurant(restaurant, command.UserId).FlatMapAsync(_ =>
            RateAsync(command).MapAsync(rating =>
            PublishEventsAsync(rating.Id, rating.RateRestaurant()))));

        private Task<Option<Domain.Entities.Restaurant, Error>> EnsureRestaurantExistsAsync(
            string restaurantId) =>
            _restaurantRepository
                .GetByIdAsync(restaurantId)
                .SomeNotNull(Error.NotFound($"Restaurant with ID: {restaurantId} does not exists!"));

        private static Option<Domain.Entities.Restaurant, Error> UserShouldNotRateOwnRestaurant(
            Domain.Entities.Restaurant restaurant,
            Guid userId) =>
            restaurant.NoneWhen(r =>
                r.OwnerId == userId,
                Error.Validation("User cannot rate he/her's own restaurant."));

        private async Task<Option<Rating, Error>> RateAsync(RateRestaurant command)
        {
            var result = await RatingRepository
                .GetByUserIdAndRestaurantIdAsync(command.UserId, command.RestaurantId);

            try
            {
                // CREATE
                if (result == default)
                {
                    var entry = Mapper.Map<Rating>(command);
                    var rating = await RatingRepository.SaveAsync(entry);
                    return rating.Some<Rating, Error>();
                }

                // UPDATE
                result.Stars = command.Stars;
                await RatingRepository.UpdateAsync(result);
                return result.Some<Rating, Error>();
            }
            catch (Exception ex)
            {
                Debug.Fail($"Un error has accrued: {ex.Message}");
                return Option.None<Rating, Error>(
                    Error.Critical("Something happens while saving the rating of the restaurant!"));
            }
        }
    }
}