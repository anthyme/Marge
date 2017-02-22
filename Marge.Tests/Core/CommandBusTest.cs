using Marge.Common;
using Marge.Common.Events;
using Marge.Core.Commands;
using Marge.Core.Commands.Models;
using Marge.Infrastructure;
using NSubstitute;
using System;
using Xunit;

namespace Marge.Tests.Core
{
    public class CommandBusTest
    {
        private CommandBus bus;
        private IEventStore eventStore;

        public CommandBusTest()
        {
            bus = new CommandBus();
            eventStore = Substitute.For<IEventStore>();
            var commandHandler = new PriceCommandHandler(eventStore);
            bus.Subscribe<ChangeDiscountCommand>(commandHandler);
            bus.Subscribe<CreatePriceCommand>(commandHandler);
        }

        [Fact]
        public void WhenCreatePriceThenPriceCreated()
        {
            var command = new CreatePriceCommand(1000, 800);

            bus.Publish(command);

            var expected = new PriceCreated(command.TargetPrice, command.Cost, 0, 0.2m);

            eventStore.Received().Save(Arg.Is<Event<PriceCreated>>(x => x.Payload.Equals(expected)));
        }

        [Fact]
        public void WhenChangeDiscountThenDiscountChanged()
        {
            var given = new object[] { new Event<PriceCreated>(
                Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, new PriceCreated(1000, 800, 0, 0.2m)) };

            eventStore.RetrieveAllEvents(Arg.Any<Guid>()).Returns(given);

            var input = new ChangeDiscountCommand(Guid.NewGuid(), 10);

            var expected = new DiscountChanged(900, 10, 0.1111111111111111111111111111m);

            eventStore.Received().Save(Arg.Is<Event<DiscountChanged>>(x => x.Payload.Equals(expected)));
        }

        [Fact]
        public void WhenChangeDiscount2ThenDiscountChanged()
        {
            var given = new object[] {
                new Event<PriceCreated>(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, 
                    new PriceCreated(1000, 800, 0, 0.2m)),
                new Event<DiscountChanged>(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, 
                    new DiscountChanged(900, 10, 0.1111111111111111111111111111m)),
            };

            eventStore.RetrieveAllEvents(Arg.Any<Guid>()).Returns(given);

            var input = new ChangeDiscountCommand(Guid.NewGuid(), 0);

            var expected = new DiscountChanged(1000, 0, 0.2m);

            eventStore.Received().Save(Arg.Is<Event<DiscountChanged>>(x => x.Payload.Equals(expected)));
        }
    }
}
