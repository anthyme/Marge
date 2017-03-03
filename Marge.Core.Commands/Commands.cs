using System;
using Marge.Infrastructure;

namespace Marge.Core.Commands
{
    public class CreatePriceCommand : Command
    {
        public override Guid CommandId { get; }
        public decimal TargetPrice { get; }
        public decimal Cost { get; }

        public CreatePriceCommand(decimal targetPrice, decimal cost, Guid? commandId = null)
        {
            CommandId = commandId ?? Guid.NewGuid();
            TargetPrice = targetPrice;
            Cost = cost;
            IsFirstCommand = true;
        }

        protected override object ValueSignature => new { CommandId, TargetPrice, Cost };
    }

    public class ChangeDiscountCommand : Command
    {
        public override Guid CommandId => PriceId;

        public Guid PriceId { get; }
        public int Discount { get; }

        public ChangeDiscountCommand(Guid priceId, int discount)
        {
            PriceId = priceId;
            Discount = discount;
        }

        protected override object ValueSignature => new { PriceId, Discount };
    }

    public class DeletePriceCommand : Command
    {
        public override Guid CommandId => PriceId;

        public Guid PriceId { get; }

        public DeletePriceCommand(Guid priceId)
        {
            PriceId = priceId;
        }
        protected override object ValueSignature => new { PriceId };
    }
}
