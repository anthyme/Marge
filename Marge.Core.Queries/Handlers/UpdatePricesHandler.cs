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

        public void Handle(Event<PriceCreated> evt)
        {
            _priceRepository.Insert(new Price(evt.Id, evt.Payload.Price, evt.Payload.Discount, evt.Payload.Profit));
        }

        public void Handle(Event<DiscountChanged> evt)
        {
            _priceRepository.UpdateDiscount(new Price(evt.Id, evt.Payload.Price, evt.Payload.Discount, evt.Payload.Profit));
        }
    }
}
