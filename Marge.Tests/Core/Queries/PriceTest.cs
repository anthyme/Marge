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
            var evt = new PriceCreated(1000, 700, 0, 0.3m);
            var wrap = new EventWrapper(Guid.NewGuid(), evt);
            handler.Handle(wrap, evt);

            priceRepository.Received().Insert(new Price(wrap.StreamId, evt.Price, evt.Discount, evt.Profit));
        }

        [Fact]
        public void GivenDiscountChangedThenVerifyUpdateDiscount()
        {
            var priceRepository = Substitute.For<IPriceRepository>();
            var handler = new UpdatePricesHandler(priceRepository);
            var evt = new DiscountChanged(1000, 0, 0.3m);
            var wrap = new EventWrapper(Guid.NewGuid(), evt);
            handler.Handle(wrap, evt);

            priceRepository.Received().UpdateDiscount(new Price(wrap.StreamId, evt.Price, evt.Discount, evt.Profit));
        }

    }
}
