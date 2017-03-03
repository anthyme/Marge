using System;
using Marge.Core.Queries.Models;

namespace Marge.Core.Queries.Handlers
{
    public interface IPriceSaver
    {
        void Create(Price price);
        void Update(Price price);
        void Delete(Guid id);
    }
}