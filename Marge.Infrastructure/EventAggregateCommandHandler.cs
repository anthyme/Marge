using System;
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
            using (var stream = CreateStream(command))
            {
                var generatedEvents = handler.Handle(command, stream.CommittedEvents).ToList();
                generatedEvents.ForEach(stream.Add);
                generatedEvents.Select(x => new EventWrapper(stream.StreamId, x)).ForEach(eventBus.Publish);
                stream.CommitChanges();
            }
        }

        private IEventStoreStream CreateStream<TCommand>(TCommand command)
        {
            var stream = command is IAggregateId aggregateId
                ? eventStore.OpenStream(aggregateId.AggregateId)
                : eventStore.CreateStream(Guid.NewGuid());
            return stream;
        }
    }
}