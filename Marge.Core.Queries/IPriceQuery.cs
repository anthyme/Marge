using System;
using System.Collections.Generic;
using Marge.Core.Queries.Models;

namespace Marge.Core.Queries
{
    public interface IPriceQuery
    {
        Price RetrieveById(Guid id);
        IEnumerable<Price> RetrieveAll();
    }
}