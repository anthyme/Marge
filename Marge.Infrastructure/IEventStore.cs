using System;
using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public interface IEventStore
    {
        IEventStoreStream CreateStream(Guid id);
        IEventStoreStream OpenStream(Guid id);
    }

    public interface IEventStoreStream : IDisposable
    {
        ICollection<Event> CommittedEvents { get; }
        Guid StreamId { get; }
        void Add(Event @event);
        void ClearChanges();
        void CommitChanges();
    }
}