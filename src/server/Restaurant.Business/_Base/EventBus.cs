using MediatR;
using Restaurant.Domain.Events._Base;
using System.Threading.Tasks;

namespace Restaurant.Business._Base
{
    public class EventBus : IEventBus
    {
        private readonly IMediator _mediator;

        public EventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Publish<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            return _mediator.Publish(@event);
        }

        public async Task Publish<TEvent>(params TEvent[] events) 
            where TEvent : IEvent
        {
            foreach (var @event in events)
            {
                await _mediator.Publish(@event);
            }
        }
    }
}
