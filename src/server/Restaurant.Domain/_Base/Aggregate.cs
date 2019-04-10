using Restaurant.Domain.Events._Base;
using System.Collections.Generic;

namespace Restaurant.Domain._Base
{
    public class Aggregate : IAggregate
    {
        public int Id { get; set; }

        public Queue<IEvent> PendingEvents { get; private set; } = new Queue<IEvent>();
    }
}