using Marge.Common;
using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public interface IHandle<TCommand>
    {
        IEnumerable<EventWrapper> Handle(TCommand command);
    }
}
