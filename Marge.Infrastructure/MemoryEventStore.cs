using System;
using Marge.Common;
using EventWithPayloads = System.Collections.Generic.IEnumerable<object>;

namespace Marge.Infrastructure
{
    public interface IEventStore
    {
        EventWithPayloads RetrieveAllEvents(Guid id);
        void Save<T>(Event<T> evt);
    }

    public class MemoryEventStore : IEventStore
    {
        public EventWithPayloads RetrieveAllEvents(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save<T>(Event<T> evt)
        {
            throw new NotImplementedException();
        }
    }
}
