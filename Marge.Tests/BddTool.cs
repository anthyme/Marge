using System;
using Marge.Core.Commands.Handlers;
using Marge.Infrastructure;
using NSubstitute;

namespace Marge.Tests
{
    public class Given
    {
        private readonly IEvent[] initialEvents;
        private IEvent[] resultingEvents;
        private object command;

        public Given(params IEvent[] initialEvents)
        {
            this.initialEvents = initialEvents;
        }

        public Given When(object command)
        {
            this.command = command;
            return this;
        }

        public void Then(params IEvent[] resultingEvents)
        {
            this.resultingEvents = resultingEvents;
            Run();
        }

        private void Run()
        {
            var eventStore = Substitute.For<IEventStore>();
            var eventBus = Substitute.For<IEventBus>();
            var transactionFactory = Substitute.For<ITransactionFactory>();
            var eventStoreStream = Substitute.For<IEventStoreStream>();
            eventStore.CreateStream(Arg.Any<Guid>()).Returns(x => eventStoreStream);
            eventStore.OpenStream(Arg.Any<Guid>()).Returns(x => eventStoreStream);
            var bus = new CommandBus(new EventAggregateCommandHandler(eventStore, eventBus, transactionFactory));
            PriceCommandHandler.RegisterCommands(bus);

            eventStoreStream.CommittedEvents.Returns(initialEvents);

            bus.Publish(command);


            if (command is IAggregateId agg)
            {
                eventStore.Received().OpenStream(agg.AggregateId);
            }

            foreach (var resultingEvent in resultingEvents)
            {

                eventStoreStream.Received().Add(resultingEvent);
                eventBus.Received().Publish(Arg.Is<EventWrapper>(x => x.Event.Equals(resultingEvent)));
            }

            eventStoreStream.Received().CommitChanges();
        }
    }
}
