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
        ICollection<IEvent> CommittedEvents { get; }
        Guid StreamId { get; }
        void Add(IEvent @event);
        void ClearChanges();
        void CommitChanges();
    }
}