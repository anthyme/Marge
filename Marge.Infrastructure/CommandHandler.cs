using System.Collections.Generic;
using Marge.Common;

namespace Marge.Infrastructure
{
    public delegate IEnumerable<IEvent> CommandHandler<in TCommand>(IEnumerable<IEvent> events, TCommand command);
}
