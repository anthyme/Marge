using System;
using System.Linq;

namespace Marge.Infrastructure
{
    public interface IEventAggregateCommandHandler
    {
        void Handle<TCommand>(CommandHandler<TCommand> handler, TCommand command);
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

        public void Handle<TCommand>(CommandHandler<TCommand> handler, TCommand command)
        {
            using (var stream = CreateStream(command))
            {
                var generatedEvents = handler(stream.CommittedEvents, command).ToList();
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