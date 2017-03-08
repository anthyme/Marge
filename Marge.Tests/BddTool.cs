using System;
using System.Collections.Immutable;
using System.Linq;
using Marge.Core.Commands;
using Marge.Infrastructure;
using Marge.Infrastructure.Data;
using NSubstitute;
using NEventStore;
using NFluent;

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
            using (var store = Wireup.Init().UsingInMemoryPersistence().Build())
            {
                var eventBus = Substitute.For<IEventBus>();

                if (!command.IsFirstCommand)
                {
                    using (var stream = store.CreateStream(command.CommandId))
                    {
                        initialEvents.Select(x => new EventMessage { Body = x }).ToList().ForEach(stream.Add);
                        stream.CommitChanges(Guid.NewGuid());
                    }
                }

                var bus = new CommandBus(new EventStoreCommandHandler(store, eventBus));
                bus.Subscribe(PriceAggregate.Handle);
                bus.Publish(command);

                using (var stream = store.OpenStream(command.CommandId))
                {
                    Check.That(stream.CommittedEvents.Select(x => x.Body).Cast<Event>()).Contains(resultingEvents);
                }
            }
        }
    }
}
