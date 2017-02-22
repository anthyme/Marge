using Marge.Common;
using Marge.Common.Events;
using Marge.Core.Queries;
using Marge.Core.Queries.Handlers;
using Marge.Core.Queries.Models;
using NSubstitute;
using System;
using Xunit;

namespace Marge.Tests.Core.Queries
{
    public class PriceTest
    {
        [Fact]
        public void GivenPriceCreatedThenVerifySave()
        {
            var priceRepository = Substitute.For<IPriceRepository>();
            var handler = new UpdatePricesHandler(priceRepository);
            var evt = new Event<PriceCreated>(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, new PriceCreated(1000, 700, 0, 0.3m));
            handler.Handle(evt);

            priceRepository.Received().Save(new Price(evt.Id, evt.Payload.Price, evt.Payload.Discount, evt.Payload.Profit));
        }

    }
}
