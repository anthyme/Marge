namespace Marge.Common.Events
{
    public struct DiscountChanged
    {
        public decimal Discount { get; }
        public decimal Profit { get; }

        public DiscountChanged(int discount, decimal profit)
        {
            Discount = discount;
            Profit = profit;
        }
    }
}
