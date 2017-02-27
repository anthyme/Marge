using Marge.Infrastructure;

namespace Marge.Common.Events
{
    public class PriceCreated : Value, IEvent
    {
        public decimal Price { get; }
        public decimal Cost { get; }
        public decimal Discount { get; }
        public decimal Profit { get; }

        public PriceCreated(decimal price, decimal cost, int discount, decimal profit)
        {
            Price = price;
            Cost = cost;
            Discount = discount;
            Profit = profit;
        }

        protected override object ValueSignature => new { Price, Cost, Discount, Profit };
    }
}
