using Marge.Common;
using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Marge.Infrastructure
{
    public interface IEventBus
    {
        void Publish(EventWrapper @event);
        IDisposable Subscribe<T>(Action<EventWrapper, T> subscription);
    }

    public class EventBus : IEventBus
    {
        private Subject<EventWrapper> subject = new Subject<EventWrapper>();

        public void Publish(EventWrapper @event)
        {
            subject.OnNext(@event);
        }

        public IDisposable Subscribe<T>(Action<EventWrapper, T> subscription)
        {
            return subject.Where(x => x.Event is T).Subscribe(x => subscription(x, (T)x.Event));
        }
    }
}
