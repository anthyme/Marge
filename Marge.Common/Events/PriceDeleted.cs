using Marge.Infrastructure;

namespace Marge.Common.Events
{
    public class PriceDeleted : Event
    {
        protected override object ValueSignature => new { };
    }
}