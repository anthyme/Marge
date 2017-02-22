using Marge.Core.Commands.Models;
using Marge.Infrastructure;
using Events = System.Collections.Generic.IEnumerable<object>;

namespace Marge.Core.Commands
{
    public class PriceCommandHandler : IHandle<CreatePriceCommand>, IHandle<ChangeDiscountCommand>
    {
        private readonly IEventStore eventStore;

        public PriceCommandHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public Events Handle(ChangeDiscountCommand command)
        {
            var price = new Price(eventStore.RetrieveAllEvents(command.PriceId));
            return price.ChangeDiscount(command);
        }

        public Events Handle(CreatePriceCommand command)
        {
            return Price.Create(command);
        }
    }
}
