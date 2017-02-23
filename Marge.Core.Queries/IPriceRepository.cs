using System;
using System.Collections.Generic;
using System.Linq;
using Marge.Core.Queries.Models;

namespace Marge.Core.Queries
{
    public interface IPriceRepository
    {
        void Insert(Price price);
        void UpdateDiscount(Price price);
    }

    public interface IPriceQuery
    {
        Price RetrieveById(Guid id);
        IEnumerable<Price> RetrieveAll();
    }

    public class PriceRepository : IPriceRepository, IPriceQuery
    {
        private readonly IDictionary<Guid, Price> prices = new Dictionary<Guid, Price>();

        public void Insert(Price price)
        {
            prices[price.Id] = price;
        }

        public void UpdateDiscount(Price price)
        {
            Insert(price);
        }

        public Price RetrieveById(Guid id)
        {
            return prices[id];
        }

        public IEnumerable<Price> RetrieveAll()
        {
            return prices.Values.ToList();
        }
    }
}
