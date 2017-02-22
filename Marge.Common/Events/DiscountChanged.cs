namespace Marge.Common.Events
{
    public struct DiscountChanged
    {
        public decimal Price { get; }
        public decimal Discount { get; }
        public decimal Profit { get; }

        public DiscountChanged(decimal price, int discount, decimal profit)
        {
            Price = price;
            Discount = discount;
            Profit = profit;
        }

        public override string ToString()
        {
            return $"Price = {Price} Discount = {Discount} Profit = {Profit}";
        }
    }
}
