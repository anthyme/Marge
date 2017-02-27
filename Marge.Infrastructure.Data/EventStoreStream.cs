using System;
using System.Collections.Generic;
using System.Linq;
using Marge.Common;
using NEventStore;

namespace Marge.Infrastructure.Data
{
    public class EventStoreStream : IEventStoreStream
    {
        private readonly IEventStream stream;

        public Guid StreamId { get; }
        public ICollection<EventWrapper> CommittedEvents => stream.CommittedEvents.Select(Extract).ToList();

        public EventStoreStream(Guid streamId, IEventStream stream)
        {
            StreamId = streamId;
            this.stream = stream;
        }

        public void Add(IEvent @event)
        {
            stream.Add(new EventMessage { Body = @event });
        }

        public void ClearChanges()
        {
            stream.ClearChanges();
        }

        public void CommitChanges()
        {
            stream.CommitChanges(Guid.NewGuid());
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        private EventWrapper Extract(EventMessage eventMessage)
        {
            return new EventWrapper(StreamId, (IEvent)eventMessage.Body);
        }
    }
}