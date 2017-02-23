using System;
using Marge.Common;
using Marge.Common.Events;
using Marge.Core.Commands;
using Marge.Core.Commands.Handlers;
using Marge.Infrastructure;
using NSubstitute;
using Xunit;

namespace Marge.Tests.Core.Commands
{
    public class CommandBusTest
    {
        private CommandBus bus;
        private IEventStore eventStore;

        public CommandBusTest()
        {
            bus = new CommandBus();
            eventStore = Substitute.For<IEventStore>();
            var commandHandler = new PriceCommandHandler(eventStore, new EventBus());
            bus.Subscribe<ChangeDiscountCommand>(commandHandler);
            bus.Subscribe<CreatePriceCommand>(commandHandler);
        }

        [Fact]
        public void WhenCreatePriceThenPriceCreated()
        {
            var command = new CreatePriceCommand(1000, 800);

            bus.Publish(command);

            var expected = new PriceCreated(command.TargetPrice, command.Cost, 0, 0.2m);

            eventStore.Received().Save(Arg.Is<EventWrapper>(x => x.Event.Equals(expected)));
        }

        [Fact]
        public void WhenChangeDiscountThenDiscountChanged()
        {
            var id = Guid.NewGuid();
            var given = new[] { new EventWrapper(id, new PriceCreated(1000, 800, 0, 0.2m)) };

            eventStore.RetrieveAllEvents(Arg.Any<Guid>()).Returns(given);

            var command = new ChangeDiscountCommand(id, 10);

            var expected = new DiscountChanged(900, 10, 0.1111111111111111111111111111m);

            bus.Publish(command);

            eventStore.Received().Save(new EventWrapper(id, expected));
        }

        [Fact]
        public void WhenChangeDiscount2ThenDiscountChanged()
        {
            var id = Guid.NewGuid();
            var given = new[] {
                new EventWrapper(id, new PriceCreated(1000, 800, 0, 0.2m)),
                new EventWrapper(id, new DiscountChanged(900, 10, 0.1111111111111111111111111111m)),
            };

            eventStore.RetrieveAllEvents(Arg.Any<Guid>()).Returns(given);

            var command = new ChangeDiscountCommand(id, 0);

            var expected = new DiscountChanged(1000, 0, 0.2m);

            bus.Publish(command);

            eventStore.Received().Save(new EventWrapper(id, expected));
        }
    }
}
