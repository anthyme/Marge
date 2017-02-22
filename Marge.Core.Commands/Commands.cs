namespace Marge.Core.Commands
{
    public class CreatePriceCommand
    {
        public decimal Price { get; }
        public decimal Cost { get; }

        public CreatePriceCommand(decimal price, decimal cost)
        {
            Price = price;
            Cost = cost;
        }
    }
}
