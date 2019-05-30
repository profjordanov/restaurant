using Restaurant.Domain.Events._Base;
using System;

namespace Restaurant.Domain.Events.User
{
    public class UserLoggedIn : IEvent
    {
        public Guid UserId { get; set; }

        public DateTime DateTime { get; set; }
    }
}