using System;
using Marge.Common;
using System.Collections.Generic;
using System.Linq;

namespace Marge.Infrastructure
{
    public interface IEventStore
    {
        IEnumerable<EventWrapper> RetrieveAllEvents(Guid id);
        void Save(EventWrapper evt);
    }

    public class InMemoryEventStore : IEventStore
    {
        private readonly List<EventWrapper> events = new List<EventWrapper>();

        public IEnumerable<EventWrapper> RetrieveAllEvents(Guid id)
        {
            return events.Where(x => x.StreamId == id).ToList();
        }

        public void Save(EventWrapper evt)
        {
            events.Add(evt);
        }
    }
}
