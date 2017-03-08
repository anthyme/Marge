using Marge.Common.Events;
using Marge.Core.Queries.Models;
using Marge.Infrastructure;

namespace Marge.Core.Queries.Handlers
{
    public class UpdatePricesHandler
    {
        private readonly IPriceSaver priceSaver;

        public UpdatePricesHandler(IPriceSaver priceSaver)
        {
            this.priceSaver = priceSaver;
        }

        public void Handle(WrappedEvent wrapper, PriceCreated evt)
        {
            priceSaver.Create(new Price(wrapper.StreamId, evt.Price, evt.Discount, evt.Profit));
        }

        public void Handle(WrappedEvent wrapper, DiscountChanged evt)
        {
            priceSaver.Update(new Price(wrapper.StreamId, evt.Price, evt.Discount, evt.Profit));
        }

        public void Handle(WrappedEvent wrapper, PriceDeleted evt)
        {
            priceSaver.Delete(wrapper.StreamId);
        }
    }
}
