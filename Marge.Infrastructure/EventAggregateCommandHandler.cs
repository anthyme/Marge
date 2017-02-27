using System;
using System.Collections.Generic;
using System.Linq;
using Marge.Common;

namespace Marge.Infrastructure
{
    public interface IEventAggregateCommandHandler
    {
        void Handle<TCommand>(IHandle<TCommand> handler, TCommand command);
    }

    public class EventAggregateCommandHandler : IEventAggregateCommandHandler
    {
        private readonly IEventStore eventStore;
        private readonly IEventBus eventBus;

        public EventAggregateCommandHandler(IEventStore eventStore, IEventBus eventBus)
        {
            this.eventStore = eventStore;
            this.eventBus = eventBus;
        }

        public void Handle<TCommand>(IHandle<TCommand> handler, TCommand command)
        {
            var (aggregateId, events) = GetAggregateInfos(command);
            var generatedEvents = handler.Handle(command, events).Select(x => new EventWrapper(aggregateId, x)).ToList();
            generatedEvents.ForEach(eventStore.Save);
            generatedEvents.ForEach(eventBus.Publish);
        }

        (Guid aggregateId, IEnumerable<IEvent> events) GetAggregateInfos<TCommand>(TCommand command)
        {
            return command is IAggregateId aggregateId
                ? (aggregateId.AggregateId, GetEvents(aggregateId.AggregateId))
                : (Guid.NewGuid(), Enumerable.Empty<IEvent>());

            IEnumerable<IEvent> GetEvents(Guid id) => eventStore.RetrieveAllEvents(id).Select(x => x.Event);
        }
    }
}