using Restaurant.Domain.Events._Base;
using System;

namespace Restaurant.Domain.Events.User
{
    public class UserRegistered : IEvent
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}