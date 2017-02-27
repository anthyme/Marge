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
            new Given()
                .When(new CreatePriceCommand(1000, 800))
                .Then(new PriceCreated(1000, 800, 0, 0.2m));
        }

        [Fact]
        public void WhenChangeDiscountThenDiscountChanged()
        {
            new Given(new PriceCreated(1000, 800, 0, 0.2m))
                .When(new ChangeDiscountCommand(Guid.NewGuid(), 10))
                .Then(new DiscountChanged(900, 10, 0.1111111111111111111111111111m));
        }

        [Fact]
        public void WhenChangeDiscount2ThenDiscountChanged()
        {
            new Given(new PriceCreated(1000, 800, 0, 0.2m),
                      new DiscountChanged(900, 10, 0.1111111111111111111111111111m))
                .When(new ChangeDiscountCommand(Guid.NewGuid(), 0))
                .Then(new DiscountChanged(1000, 0, 0.2m));
        }
    }
}
