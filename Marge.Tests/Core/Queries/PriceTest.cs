using Marge.Common.Events;
using Marge.Core.Queries;
using Marge.Core.Queries.Handlers;
using Marge.Core.Queries.Models;
using NSubstitute;
using System;
using Marge.Infrastructure;
using Xunit;

namespace Marge.Tests.Core.Queries
{
    public class PriceTest
    {
        [Fact]
        public void GivenPriceCreatedThenVerifySave()
        {
            var priceSaver = Substitute.For<IPriceSaver>();
            var handler = new UpdatePricesHandler(priceSaver);
            var evt = new PriceCreated(1000, 700, 0, 0.3m);
            var wrap = new EventWrapper(Guid.NewGuid(), evt);
            handler.Handle(wrap, evt);

            priceSaver.Received().Create(new Price(wrap.StreamId, evt.Price, evt.Discount, evt.Profit));
        }

        [Fact]
        public void GivenDiscountChangedThenVerifyUpdateDiscount()
        {
            var priceSaver = Substitute.For<IPriceSaver>();
            var handler = new UpdatePricesHandler(priceSaver);
            var evt = new DiscountChanged(1000, 0, 0.3m);
            var wrap = new EventWrapper(Guid.NewGuid(), evt);
            handler.Handle(wrap, evt);

            priceSaver.Received().Update(new Price(wrap.StreamId, evt.Price, evt.Discount, evt.Profit));
        }
    }
}
