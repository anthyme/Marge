using Marge.Infrastructure;

namespace Marge.Common.Events
{
    public class DiscountChanged : Event
    {
        public decimal Price { get; }
        public int Discount { get; }
        public decimal Profit { get; }

        public DiscountChanged(decimal price, int discount, decimal profit)
        {
            Price = price;
            Discount = discount;
            Profit = profit;
        }

        protected override object ValueSignature => new { Price, Discount, Profit };
    }
}
