using Marge.Common;
using Marge.Core.Commands.Models;
using Marge.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marge.Core.Commands
{
    public class PriceCommandHandler : IHandle<CreatePriceCommand>, IHandle<ChangeDiscountCommand>
    {
        private readonly IEventStore eventStore;

        public PriceCommandHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public IEnumerable<EventWrapper> Handle(ChangeDiscountCommand command)
        {
            var price = new Price(eventStore.RetrieveAllEvents(command.PriceId).Select(x => x.Event).ToArray());
            var payloads = price.ChangeDiscount(command);
            var events = payloads.Select(x => new EventWrapper(Guid.NewGuid(), x)).ToList();
            events.ForEach(eventStore.Save);
            return events;
        }

        public IEnumerable<EventWrapper> Handle(CreatePriceCommand command)
        {
            var payloads = Price.Create(command);
            var events = payloads.Select(x => new EventWrapper(Guid.NewGuid(), x)).ToList();
            events.ForEach(eventStore.Save);
            return events;
        }
    }
}
