using System;
using Marge.Infrastructure;

namespace Marge.Core.Commands
{
    public class CreatePriceCommand
    {
        public decimal TargetPrice { get; }
        public decimal Cost { get; }

        public CreatePriceCommand(decimal targetPrice, decimal cost)
        {
            TargetPrice = targetPrice;
            Cost = cost;
        }
    }

    public class ChangeDiscountCommand : IAggregateId
    {
        Guid IAggregateId.AggregateId => PriceId;

        public Guid  PriceId { get; }
        public int Discount { get; }

        public ChangeDiscountCommand(Guid priceId, int discount)
        {
            PriceId = priceId;
            Discount = discount;
        }
    }
}
