using System;
using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public interface IEventBus
    {
        void Publish(WrappedEvent @event);
        void Subscribe<T>(Action<WrappedEvent, T> subscription) where T : Event;
    }

    public class EventBus : IEventBus
    {
        private readonly IDictionary<Type, List<Action<object>>> subscriptions = new Dictionary<Type, List<Action<object>>>();

        public void Publish(WrappedEvent @event)
        {
            subscriptions[@event.Event.GetType()].ForEach(subscription => subscription(@event));
        }

        public void Subscribe<T>(Action<WrappedEvent, T> subscription) where T : Event
        {
            if (!subscriptions.ContainsKey(typeof(T)))
            {
                subscriptions[typeof(T)] = new List<Action<object>>();
            }

            subscriptions[typeof(T)].Add(x =>
            {
                var evt = (WrappedEvent) x;
                subscription(evt, (T) evt.Event);
            });
        }
    }
}
