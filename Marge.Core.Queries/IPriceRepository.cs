using Marge.Core.Queries.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marge.Core.Queries
{
    public interface IPriceRepository
    {
        void Save(Price price);

        Price Get(Guid id);
    }
}
