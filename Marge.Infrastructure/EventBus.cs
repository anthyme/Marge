using Marge.Common;
using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Marge.Infrastructure
{
    public class EventBus
    {
        public static EventBus Instance => new EventBus();

        private Subject<object> subject = new Subject<object>();

        public void Publish<T>(Event<T> @event)
        {
            subject.OnNext(@event);
        }

        public IDisposable Subscribe<T>(Action<Event<T>> subscription)
        {
            return subject.OfType<Event<T>>().Subscribe(subscription);
        }
    }
}
