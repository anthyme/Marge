using Marge.Infrastructure;

namespace Marge.Common.Events
{
    public class PriceDeleted : Value, IEvent
    {
        protected override object ValueSignature => new { };
    }
}