using AutoMapper;
using FluentValidation;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Optional;
using Optional.Async;
using Restaurant.Core.AuthContext;
using Restaurant.Core.AuthContext.Commands;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Events._Base;
using Restaurant.Persistence.EntityFramework;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Business.AuthContext.CommandHandlers
{
    public class RegisterHandler : BaseAuthHandler<Register>
    {
        public RegisterHandler(
            IValidator<Register> validator,
            ApplicationDbContext dbContext,
            IDocumentSession documentSession,
            IEventBus eventBus,
            IMapper mapper,
            UserManager<User> userManager,
            IJwtFactory jwtFactory) 
            : base(validator, dbContext, documentSession, eventBus, mapper, userManager, jwtFactory)
        {
        }

        public override Task<Option<Unit, Error>> Handle(Register command) =>
            CheckIfUserDoesntExist(command.Email).FlatMapAsync(_ =>
            PersistUser(command)).MapAsync(user =>
            PublishEventsAsync(user.Id, user.RegisterUser()));

        private async Task<Option<User, Error>> PersistUser(Register command)
        {
            var user = Mapper.Map<User>(command);

            var creationResult = (await UserManager.CreateAsync(user, command.Password))
                .SomeWhen(
                    x => x.Succeeded,
                    x => Error.Validation(x.Errors.Select(e => e.Description)));

            return creationResult
                .Map(_ => user);
        }

        private async Task<Option<User, Error>> CheckIfUserDoesntExist(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);

            return user
                .SomeWhen(
                    u => u == null,
                    Error.Conflict($"User with email {email} already exists."));
        }
    }
}