using Marge.Common;
using Marge.Common.Events;
using Marge.Core.Queries.Models;

namespace Marge.Core.Queries.Handlers
{
    public class UpdatePricesHandler
    {
        private readonly PriceRepository priceSaver;

        public UpdatePricesHandler(PriceRepository priceSaver)
        {
            this.priceSaver = priceSaver;
        }

        public void Handle(EventWrapper wrapper, PriceCreated evt)
        {
            priceSaver.Create(new Price(wrapper.StreamId, evt.Price, evt.Discount, evt.Profit));
        }

        public void Handle(EventWrapper wrapper, DiscountChanged evt)
        {
            priceSaver.Update(new Price(wrapper.StreamId, evt.Price, evt.Discount, evt.Profit));
        }
    }
}
