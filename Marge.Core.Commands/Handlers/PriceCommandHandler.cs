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
            return WrapSavePublish(Guid.NewGuid(), Price.Create(command));
        }

        public IEnumerable<EventWrapper> Handle(ChangeDiscountCommand command)
        {
            var id = command.PriceId;
            var price = new Price(eventStore.RetrieveAllEvents(id).Select(x => x.Event));
            var payloads = price.ChangeDiscount(command);

            return WrapSavePublish(id, payloads);
        }

        private List<EventWrapper> WrapSavePublish(Guid id, IEnumerable<IEvent> payloads)
        {
            var events = payloads.Select(x => new EventWrapper(id, x)).ToList();
            events.ForEach(eventStore.Save);
            events.ForEach(eventBus.Publish);
            return events;
        }
    }
}
