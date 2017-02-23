using System;
using System.Collections.Generic;
using System.Linq;
using Marge.Common;
using Marge.Core.Commands.Models;
using Marge.Infrastructure;

namespace Marge.Core.Commands.Handlers
{
    public class PriceCommandHandler : IHandle<CreatePriceCommand>, IHandle<ChangeDiscountCommand>
    {
        private readonly IEventStore eventStore;
        private readonly EventBus eventBus;

        public PriceCommandHandler(IEventStore eventStore, EventBus eventBus)
        {
            this.eventStore = eventStore;
            this.eventBus = eventBus;
        }

        public IEnumerable<EventWrapper> Handle(CreatePriceCommand command)
        {
            var payloads = Price.Create(command);
            var events = payloads.Select(x => new EventWrapper(Guid.NewGuid(), x)).ToList();
            events.ForEach(eventStore.Save);
            events.ForEach(eventBus.Publish);
            return events;
        }

        public IEnumerable<EventWrapper> Handle(ChangeDiscountCommand command)
        {
            var price = new Price(eventStore.RetrieveAllEvents(command.PriceId).Select(x => x.Event).ToArray());
            var payloads = price.ChangeDiscount(command);
            var events = payloads.Select(x => new EventWrapper(command.PriceId, x)).ToList();
            events.ForEach(eventStore.Save);
            events.ForEach(eventBus.Publish);
            return events;
        }
    }
}
