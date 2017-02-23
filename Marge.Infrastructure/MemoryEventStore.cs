using System;
using Marge.Common;
using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public interface IEventStore
    {
        IEnumerable<EventWrapper> RetrieveAllEvents(Guid id);
        void Save(EventWrapper evt);
    }

}
