using System;
using System.Collections.Generic;
using System.Linq;
using Marge.Core.Queries.Models;

namespace Marge.Core.Queries
{
    public interface IPriceQuery
    {
        Price RetrieveById(Guid id);
        IEnumerable<Price> RetrieveAll();
    }

    public interface IPriceSaver
    {
        void Create(Price price);
        void Update(Price price);
    }

    public class PriceRepository : IPriceQuery, IPriceSaver
    {
        private readonly IDictionary<Guid, Price> prices = new Dictionary<Guid, Price>();

        public Price RetrieveById(Guid id)
        {
            return prices[id];
        }

        public IEnumerable<Price> RetrieveAll()
        {
            return prices.Values.ToList();
        }

        public void Create(Price price)
        {
            prices[price.Id] = price;
        }

        public void Update(Price price)
        {
            Create(price);
        }
    }
}
