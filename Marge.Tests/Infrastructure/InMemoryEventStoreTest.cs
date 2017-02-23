using System;
using System.Linq;
using Marge.Common;
using Marge.Common.Events;
using Marge.Infrastructure;
using NFluent;
using Xunit;

namespace Marge.Tests.Infrastructure
{
    public class InMemoryEventStoreTest
    {
        private readonly InMemoryEventStore sut;

        public InMemoryEventStoreTest()
        {
            sut = new InMemoryEventStore();
        }

        [Fact]
        public void WhenSavingEventThenTheyCanBeRetrieved()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var events = new[]
            {
                new EventWrapper(id1, new PriceCreated(10, 10, 10, 10)),
                new EventWrapper(id1, new DiscountChanged(10, 10, 10)),
                new EventWrapper(id2, new PriceCreated(20, 10, 10, 10)),
                new EventWrapper(id1, new DiscountChanged(10, 5, 1)),
                new EventWrapper(id2, new DiscountChanged(20, 5, 1)),
                new EventWrapper(id1, new DiscountChanged(10, 5, 1)),
                new EventWrapper(id1, new DiscountChanged(10, 20, 30)),
            };


            var expected = new[]
            {
                new EventWrapper(id2, new PriceCreated(20, 10, 10, 10)),
                new EventWrapper(id2, new DiscountChanged(20, 5, 1)),
            };

            events.ToList().ForEach(sut.Save);

            Check.That(sut.RetrieveAllEvents(id2)).ContainsExactly(expected);
        }
    }
}
