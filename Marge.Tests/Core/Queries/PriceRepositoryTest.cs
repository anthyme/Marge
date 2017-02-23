using System;
using System.Linq;
using Marge.Core.Queries;
using Marge.Core.Queries.Models;
using NFluent;
using Xunit;

namespace Marge.Tests.Core.Queries
{
    public class PriceRepositoryTest
    {
        private readonly PriceRepository sut;

        public PriceRepositoryTest()
        {
            sut = new PriceRepository();
        }

        [Fact]
        public void WhenInsertingThenICanGetThemAll()
        {
            var prices = new[]
            {
                new Price(Guid.NewGuid(), 10, 20, 30),
                new Price(Guid.NewGuid(), 11, 12, 13),
            };

            prices.ToList().ForEach(sut.Create);


            Check.That(sut.RetrieveAll()).ContainsExactly(prices);
        }


        [Fact]
        public void WhenInsertingThenICanRetrieveOne()
        {
            var prices = new[]
            {
                new Price(Guid.NewGuid(), 10, 20, 30),
                new Price(Guid.NewGuid(), 11, 12, 13),
            };

            prices.ToList().ForEach(sut.Create);


            Check.That(sut.RetrieveById(prices[1].Id)).Equals(prices[1]);
        }

        [Fact]
        public void WhenUpdatingThenICanGetThemAll()
        {
            var id = Guid.NewGuid();
            var prices = new[]
            {
                new Price(id, 10, 20, 30),
                new Price(Guid.NewGuid(), 11, 12, 13),
            };

            prices.ToList().ForEach(sut.Create);

            var update = new Price(id, 12, 22, 33);

            sut.Update(update);

            Check.That(sut.RetrieveById(id)).Equals(update);
        }
    }
}
