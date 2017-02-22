using Events = System.Collections.Generic.IEnumerable<object>;

namespace Marge.Infrastructure
{
    public interface IHandle<TCommand>
    {
        Events Handle(TCommand command);
    }
}
