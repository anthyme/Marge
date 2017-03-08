using System;
using System.Linq;
using System.Transactions;
using NEventStore;

namespace Marge.Infrastructure.Data
{
    public class EventStoreCommandHandler : IEventStoreCommandHandler
    {
        private readonly IStoreEvents store;
        private readonly IEventBus eventBus;

        public EventStoreCommandHandler(IStoreEvents store, IEventBus eventBus)
        {
            this.store = store;
            this.eventBus = eventBus;
        }

        public void Handle(CommandHandler handler, Command command)
        {
            using (var stream = CreateStream(command))
            {
                var generatedEvents = handler(stream.CommittedEvents.Select(x => x.Body).Cast<Event>().ToList(), command).ToList();
                generatedEvents.ForEach(x => stream.Add(new EventMessage { Body = x }));
                using (var transaction = new TransactionScope())
                {
                    stream.CommitChanges(Guid.NewGuid());
                    generatedEvents.Select(x => new WrappedEvent(Guid.Parse(stream.StreamId), x)).ForEach(eventBus.Publish);
                    transaction.Complete();
                }
            }
        }

        private IEventStream CreateStream(Command cmd)
        {
            return cmd.IsFirstCommand ? store.CreateStream(cmd.CommandId) : store.OpenStream(cmd.CommandId);
        }
    }
}