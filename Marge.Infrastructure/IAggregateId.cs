using System;

namespace Marge.Infrastructure
{
    public interface IAggregateId
    {
        Guid AggregateId { get; }
    }
}