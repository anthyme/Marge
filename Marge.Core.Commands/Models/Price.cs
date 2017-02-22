using Marge.Common.Events;
using Events = System.Collections.Generic.IEnumerable<object>;

namespace Marge.Core.Commands.Models
{
    //TODO: make private
    public class Price
    {
        private readonly State state;

        public Price(params object[] events)
        {
            state = new State(events);
        }

        public static Events Create(CreatePriceCommand command)
        {
            yield return new PriceCreated(command.TargetPrice, command.Cost, 0, ComputeProfit(command.TargetPrice, command.Cost));
        }

        public Events ChangeDiscount(ChangeDiscountCommand command)
        {
            var price = state.TargetPrice - (state.TargetPrice * 0.01m * command.Discount);
            yield return new DiscountChanged(price, command.Discount, ComputeProfit(price, state.Cost));
        }

        private static decimal ComputeProfit(decimal price, decimal cost)
        {
            return 1 - cost / price;
        }

        class State
        {
            public decimal Cost { get; private set; }
            public decimal TargetPrice { get; private set; }

            public State(object[] events)
            {
                foreach (var evt in events)
                {
                    switch (evt)
                    {
                        case PriceCreated x: Apply(x); break;
                    }
                }
            }

            private void Apply(PriceCreated evt)
            {
                Cost = evt.Cost;
                TargetPrice = evt.Price;
            }
        }
    }
}
