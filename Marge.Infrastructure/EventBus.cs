﻿using Marge.Common;
using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Marge.Infrastructure
{
    public class EventBus
    {
        public static EventBus Instance => new EventBus();

        private Subject<EventWrapper> subject = new Subject<EventWrapper>();

        public void Publish<T>(EventWrapper @event)
        {
            subject.OnNext(@event);
        }

        public IDisposable Subscribe<T>(Action<EventWrapper, T> subscription)
        {
            return subject.Where(x => x.Event is T).Subscribe(x => subscription(x, (T)x.Event));
        }
    }
}
