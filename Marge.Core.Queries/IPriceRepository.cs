using Marge.Core.Queries.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marge.Core.Queries
{
    public interface IPriceRepository
    {
        void Insert(Price price);

        void UpdateDiscount(Price price); 

        Price Get(Guid id);
    }
}
