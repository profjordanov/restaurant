using System.Threading.Tasks;

namespace Restaurant.Domain.Events._Base
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event)
            where TEvent : IEvent;

        Task Publish<TEvent>(params TEvent[] events)
            where TEvent : IEvent;
    }
}