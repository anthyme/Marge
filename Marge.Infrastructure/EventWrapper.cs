using System;

namespace Marge.Infrastructure
{
    public abstract class Event : Value { }

    public struct EventWrapper 
    {
        public Guid StreamId { get; }
        public Event Event { get; }

        public EventWrapper(Guid streamId, Event @event)
        {
            StreamId = streamId;
            Event = @event;
        }
    }
}
