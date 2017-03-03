using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public delegate IEnumerable<Event> CommandHandler<in TCommand>(IEnumerable<Event> events, TCommand command);
}
