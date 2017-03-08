using System;
using Marge.Core.Commands;
using Marge.Infrastructure;
using Marge.Infrastructure.Data;
using NSubstitute;

namespace Marge.Tests
{
    public class Given
    {
        private readonly Event[] initialEvents;
        private Event[] resultingEvents;
        private Command command;

        public Given(params Event[] initialEvents)
        {
            this.initialEvents = initialEvents;
        }

        public Given When(Command command)
        {
            this.command = command;
            return this;
        }

        public void Then(params Event[] resultingEvents)
        {
            this.resultingEvents = resultingEvents;
            Run();
        }

        private void Run()
        {
            var eventStore = Substitute.For<IEventStore>();
            var eventBus = Substitute.For<IEventBus>();
            var eventStoreStream = Substitute.For<IEventStoreStream>();
            eventStore.CreateStream(Arg.Any<Guid>()).Returns(x => eventStoreStream);
            eventStore.OpenStream(Arg.Any<Guid>()).Returns(x => eventStoreStream);
            var bus = new CommandBus(new EventAggregateCommandHandler(eventStore, eventBus));
            bus.Subscribe(PriceAggregate.Handle);
            eventStoreStream.CommittedEvents.Returns(initialEvents);

            bus.Publish(command);


            if (command is Command cmd && !cmd.IsFirstCommand)
            {
                eventStore.Received().OpenStream(cmd.CommandId);
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
