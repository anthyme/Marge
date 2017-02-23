using Marge.Common;
using Marge.Common.Events;
using Marge.Core.Queries.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marge.Core.Queries.Handlers
{
    public class UpdatePricesHandler
    {
        private readonly IPriceRepository _priceRepository;

        public UpdatePricesHandler(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }

        public void Handle(EventWrapper wrapper, PriceCreated evt)
        {
            _priceRepository.Insert(new Price(wrapper.StreamId, evt.Price, evt.Discount, evt.Profit));
        }

        public void Handle(EventWrapper wrapper, DiscountChanged evt)
        {
            _priceRepository.UpdateDiscount(new Price(wrapper.StreamId, evt.Price, evt.Discount, evt.Profit));
        }
    }
}
