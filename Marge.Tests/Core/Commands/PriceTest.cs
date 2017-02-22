using Marge.Common.Events;
using Marge.Core.Commands;
using Marge.Core.Commands.Models;
using NFluent;
using Xunit;

namespace Marge.Tests.Core.Commands
{
    public class PriceTest
    {
        [Fact]
        public void WhenCreatePriceThenPriceCreated()
        {
            var input = new CreatePriceCommand(1000, 800);

            Check.That(Price.Create(input)).ContainsExactly(new PriceCreated(input.Price, input.Cost, 0, 0.2m));
        }
    }
}
