using System.Linq;
using System.Transactions;

namespace Marge.Infrastructure.Data
{
    public class EventAggregateCommandHandler : IEventAggregateCommandHandler
    {
        private readonly IEventStore eventStore;
        private readonly IEventBus eventBus;

        public EventAggregateCommandHandler(IEventStore eventStore, IEventBus eventBus)
        {
            this.eventStore = eventStore;
            this.eventBus = eventBus;
        }

        public void Handle(CommandHandler handler, Command command)
        {
            using (var stream = CreateStream(command))
            {
                var generatedEvents = handler(stream.CommittedEvents, command).ToList();
                generatedEvents.ForEach(stream.Add);
                using (var transaction = new TransactionScope())
                {
                    stream.CommitChanges();
                    generatedEvents.Select(x => new EventWrapper(stream.StreamId, x)).ForEach(eventBus.Publish);
                    transaction.Complete();
                }
            }
        }

        private IEventStoreStream CreateStream<TCommand>(TCommand cmd) where TCommand : Command
        {
            return cmd.IsFirstCommand ? eventStore.CreateStream(cmd.CommandId) : eventStore.OpenStream(cmd.CommandId);
        }
    }
}