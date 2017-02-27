using System.Collections.Generic;
using Marge.Common;
using Marge.Core.Commands.Models;
using Marge.Infrastructure;

namespace Marge.Core.Commands.Handlers
{
    public class PriceCommandHandler : IHandle<CreatePriceCommand>, IHandle<ChangeDiscountCommand>
    {
        public IEnumerable<IEvent> Handle(CreatePriceCommand command, IEnumerable<IEvent> events)
        {
            return Price.Create(command);
        }

        public IEnumerable<IEvent> Handle(ChangeDiscountCommand command, IEnumerable<IEvent> events)
        {
            return new Price(events).ChangeDiscount(command);
        }
    }
}
