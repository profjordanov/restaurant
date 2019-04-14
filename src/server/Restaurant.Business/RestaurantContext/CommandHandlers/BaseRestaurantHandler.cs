using AutoMapper;
using FluentValidation;
using Marten;
using Microsoft.AspNetCore.Http;
using Restaurant.Business._Base;
using Restaurant.Core._Base;
using Restaurant.Domain.Events._Base;
using Restaurant.Persistence.EntityFramework;

namespace Restaurant.Business.RestaurantContext.CommandHandlers
{
    public abstract class BaseRestaurantHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        protected BaseRestaurantHandler(
            IValidator<TCommand> validator, 
            ApplicationDbContext dbContext, 
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper) 
            : base(validator, dbContext, documentSession, eventBus, mapper)
        {
        }
    }
}