using Marge.Common;
using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public interface IHandle<in TCommand>
    {
        IEnumerable<IEvent> Handle(TCommand command, IEnumerable<IEvent> events);
    }
}
