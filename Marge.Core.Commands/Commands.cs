using System;

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

    public class ChangeDiscountCommand
    {
        public Guid  PriceId { get; }
        public int Discount { get; }

        public ChangeDiscountCommand(Guid id, int discount)
        {
            PriceId = id;
            Discount = discount;
        }
    }
}
