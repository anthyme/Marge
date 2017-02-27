using Marge.Common.Events;
using Marge.Core.Commands;
using Marge.Core.Commands.Models;
using NFluent;
using System;
using System.Collections.Generic;
using Marge.Common;
using Xunit;

namespace Marge.Tests.Core.Commands
{
    public class PriceTest
    {
        [Fact]
        public void WhenCreatePriceThenPriceCreated()
        {
            var input = new CreatePriceCommand(1000, 800);

            Check.That(Price.Create(input)).ContainsExactly(new PriceCreated(input.TargetPrice, input.Cost, 0, 0.2m));
        }

        [Fact]
        public void WhenChangeDiscountThenDiscountChanged()
        {
            var input = new ChangeDiscountCommand(Guid.NewGuid(), 10);

            var sut = new Price(new IEvent[] { new PriceCreated(1000, 800, 0, 0.2m) });

            Check.That(sut.ChangeDiscount(input))
                .ContainsExactly(new DiscountChanged(900, 10, 0.1111111111111111111111111111m));
        }

        [Fact]
        public void WhenChangeDiscount2ThenDiscountChanged()
        {
            var input = new ChangeDiscountCommand(Guid.NewGuid(), 0);

            var sut = new Price(new IEvent[] { new PriceCreated(1000, 800, 0, 0.2m),
                                               new DiscountChanged(900, 10, 0.1111111111111111111111111111m) });

            Check.That(sut.ChangeDiscount(input))
                .ContainsExactly(new DiscountChanged(1000, 0, 0.2m));
        }
    }
}
