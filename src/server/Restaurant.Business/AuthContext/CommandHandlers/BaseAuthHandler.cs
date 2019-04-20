using AutoMapper;
using FluentValidation;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Optional;
using Optional.Async;
using Restaurant.Business._Base;
using Restaurant.Core._Base;
using Restaurant.Core.AuthContext;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Events._Base;
using Restaurant.Persistence.EntityFramework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Restaurant.Business.AuthContext.CommandHandlers
{
    public abstract class BaseAuthHandler<TCommand> : BaseHandler<TCommand>
        where TCommand : ICommand
    {
        protected ApplicationDbContext DbContext { get; }
        protected UserManager<User> UserManager { get; }
        protected IJwtFactory JwtFactory { get; }

        protected BaseAuthHandler(
            IValidator<TCommand> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            UserManager<User> userManager,
            IJwtFactory jwtFactory) 
            : base(validator, documentSession, eventBus, mapper)
        {
            DbContext = dbContext;
            UserManager = userManager;
            JwtFactory = jwtFactory;
        }

        protected Task<Option<User, Error>> AccountShouldExist(Guid accountId) =>
            UserManager
                .FindByIdAsync(accountId.ToString())
                .SomeNotNull(Error.NotFound($"No account with id {accountId} was found."));

        protected async Task<Unit> AddClaim(User account, string claimType, string claimValue)
        {
            var claimToAdd = new Claim(claimType, claimValue);

            await UserManager.AddClaimAsync(account, claimToAdd);

            return Unit.Value;
        }
    }
}