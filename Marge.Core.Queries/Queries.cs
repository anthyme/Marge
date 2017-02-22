using Marge.Core.Queries.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marge.Core.Queries
{
    public class Queries
    {
        private readonly IPriceRepository _priceRepository;

        public Queries(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }

        public Price GetPrice(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
