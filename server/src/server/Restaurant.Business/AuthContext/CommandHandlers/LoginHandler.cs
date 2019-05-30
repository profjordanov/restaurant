using FluentValidation;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Optional;
using Optional.Async;
using Restaurant.Business._Base;
using Restaurant.Core._Base;
using Restaurant.Core.AuthContext;
using Restaurant.Core.AuthContext.Commands;
using Restaurant.Domain;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Events._Base;
using Restaurant.Domain.Events.User;
using Restaurant.Domain.Views.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Business.AuthContext.CommandHandlers
{
    public class LoginHandler : ICommandHandler<Login, JwtView>
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<Login> _validator;
        private readonly IDocumentSession _session;
        private readonly IEventBus _eventBus;

        public LoginHandler(
            UserManager<User> userManager, 
            IJwtFactory jwtFactory,
            IValidator<Login> validator,
            IDocumentSession session,
            IEventBus eventBus)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _validator = validator;
            _session = session;
            _eventBus = eventBus;
        }

        public Task<Option<JwtView, Error>> Handle(Login command, CancellationToken cancellationToken = default) =>
            ValidateCommand(command).FlatMapAsync(cmd =>
            FindUser(command.Email).FlatMapAsync(user =>
            CheckPassword(user, command.Password).FlatMapAsync(_ =>
            PublishEventAsync(user.Id, user.LogInUser()).FlatMapAsync(__ =>
            GetExtraClaims(user).MapAsync(async claims =>
            GenerateJwt(user, claims))))));

        private async Task<Option<bool, Error>> CheckPassword(User user, string password)
        {
            var passwordIsValid = await _userManager
                .CheckPasswordAsync(user, password);

            var result = passwordIsValid
                .SomeWhen(isValid => isValid, Error.Unauthorized("Invalid credentials."));

            return result;
        }

        private Task<Option<User, Error>> FindUser(string email) =>
            _userManager
                .FindByEmailAsync(email)
                .SomeNotNull(Error.NotFound($"No user with email {email} was found."));

        private JwtView GenerateJwt(User user, IEnumerable<Claim> extraClaims) =>
            new JwtView
            {
                TokenString = _jwtFactory.GenerateEncodedToken(user.Id.ToString(), user.Email, extraClaims)
            };

        private Task<Option<IList<Claim>, Error>> GetExtraClaims(User user) =>
                    user.SomeNotNull(Error.Validation($"You must provide a non-null user."))
                .MapAsync(u => _userManager.GetClaimsAsync(u));

        // TODO: This is duplicated in BaseHandler.cs
        private Option<Login, Error> ValidateCommand(Login command)
        {
            var validationResult = _validator.Validate(command);

            return validationResult
                .SomeWhen(
                    r => r.IsValid,
                    r => Error.Validation(r.Errors.Select(e => e.ErrorMessage)))

                // If the validation result is successful, disregard it and simply return the command
                .Map(_ => command);
        }

        private async Task<Option<Unit, Error>> PublishEventAsync(Guid streamId, UserLoggedIn @event)
        {
            _session.Events.Append(streamId, @event);
            await _session.SaveChangesAsync();
            await _eventBus.Publish(@event);

            return Unit.Value.Some<Unit, Error>();
        }
    }
}