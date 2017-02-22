using System;
using System.Collections.Generic;
using System.Text;

namespace Marge.Common
{
    public class Event<T>
    {
        public Guid Id { get; }
        public Guid StreamId { get; }
        public DateTime Date { get; }
        public T Payload { get; }

        public Event(Guid id, Guid streamId, DateTime date, T payload)
        {
            Id = id;
            StreamId = streamId;
            Date = date;
            Payload = payload;
        }
    }
}
