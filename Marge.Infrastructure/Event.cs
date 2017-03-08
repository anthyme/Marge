using System;

namespace Marge.Infrastructure
{
    public abstract class Event : Value { }

    public struct WrappedEvent 
    {
        public Guid StreamId { get; }
        public Event Event { get; }

        public WrappedEvent(Guid streamId, Event @event)
        {
            StreamId = streamId;
            Event = @event;
        }
    }
}
