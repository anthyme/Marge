using System;
using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public interface IEventBus
    {
        void Publish(EventWrapper @event);
        void Subscribe<T>(Action<EventWrapper, T> subscription);
    }

    public class EventBus : IEventBus
    {
        private readonly IDictionary<Type, List<Action<object>>> subscriptions = new Dictionary<Type, List<Action<object>>>();

        public void Publish(EventWrapper @event)
        {
            subscriptions[@event.Event.GetType()].ForEach(subscription => subscription(@event));
        }

        public void Subscribe<T>(Action<EventWrapper, T> subscription)
        {
            if (!subscriptions.ContainsKey(typeof(T)))
            {
                subscriptions[typeof(T)] = new List<Action<object>>();
            }

            subscriptions[typeof(T)].Add(x =>
            {
                var evt = (EventWrapper) x;
                subscription(evt, (T) evt.Event);
            });
        }
    }
}
