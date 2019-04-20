using AutoMapper;
using FluentValidation;
using MediatR;
using Optional;
using Restaurant.Core.RatingContext.Commands;
using Restaurant.Domain;
using Restaurant.Domain.Events._Base;
using Restaurant.Persistence.EntityFramework;
using System.Threading.Tasks;
using Marten;

namespace Restaurant.Business.RatingContext.CommandHandlers
{
    public class RateRestaurantHandler : BaseRatingHandler<RateRestaurant>
    {
        public RateRestaurantHandler(
            IValidator<RateRestaurant> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper) 
            : base(validator, documentSession, eventBus, mapper)
        {
        }

        public override Task<Option<Unit, Error>> Handle(RateRestaurant command)
        {
            throw new System.NotImplementedException();
        }

    }
}