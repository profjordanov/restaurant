using Microsoft.AspNetCore.Identity;
using Restaurant.Domain._Base;
using Restaurant.Domain.Events.User;
using System;
using System.Collections.Generic;

namespace Restaurant.Domain.Entities
{
    public class User : IdentityUser<Guid>, IAggregate
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Rating> GivenRatings { get; set; } = new HashSet<Rating>();

        public virtual ICollection<Order> MadeOrders { get; set; } = new HashSet<Order>();

        public virtual ICollection<Restaurant> OwnedRestaurants { get; set; } = new HashSet<Restaurant>();

        // Events
        public UserLoggedIn LogInUser() => new UserLoggedIn
        {
            UserId = Id,
            DateTime = DateTime.UtcNow
        };

        public UserRegistered RegisterUser() => new UserRegistered
        {
            UserId = Id,
            FirstName = FirstName,
            LastName = LastName,
            RegistrationDate = RegistrationDate
        };

        public void Apply(UserLoggedIn @event)
        {
            Id = @event.UserId;
        }

        public void Apply(UserRegistered @event)
        {
            Id = @event.UserId;
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            RegistrationDate = @event.RegistrationDate;
        }
    }
}
