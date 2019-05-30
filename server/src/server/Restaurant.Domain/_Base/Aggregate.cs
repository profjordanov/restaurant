using Restaurant.Domain.Events._Base;
using System;
using System.Collections.Generic;

namespace Restaurant.Domain._Base
{
    public class Aggregate
    {
        public Guid Id { get; protected set; }

        public Queue<IEvent> PendingEvents { get; private set; } = new Queue<IEvent>();
    }
}