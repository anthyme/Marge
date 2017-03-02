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
        private readonly ITransactionFactory transactionFactory;

        public EventAggregateCommandHandler(IEventStore eventStore, IEventBus eventBus, ITransactionFactory transactionFactory)
        {
            this.eventStore = eventStore;
            this.eventBus = eventBus;
            this.transactionFactory = transactionFactory;
        }

        public void Handle<TCommand>(CommandHandler<TCommand> handler, TCommand command)
        {
            using (var stream = CreateStream(command))
            {
                var generatedEvents = handler(stream.CommittedEvents, command).ToList();
                generatedEvents.ForEach(stream.Add);
                using (var transaction = transactionFactory.Create())
                {
                    stream.CommitChanges();
                    generatedEvents.Select(x => new EventWrapper(stream.StreamId, x)).ForEach(eventBus.Publish);
                    transaction.Complete();
                }
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