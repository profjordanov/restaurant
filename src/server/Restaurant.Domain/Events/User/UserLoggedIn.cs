using System;
using Restaurant.Domain.Events._Base;

namespace Restaurant.Domain.Events.User
{
    public class UserLoggedIn : IEvent
    {
        public Guid UserId { get; set; }

        public DateTime DateTime { get; set; }
    }
}