using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public delegate IEnumerable<IEvent> CommandHandler<in TCommand>(IEnumerable<IEvent> events, TCommand command);
}
