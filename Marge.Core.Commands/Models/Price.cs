using Marge.Common.Events;
using System.Collections.Generic;
using Marge.Infrastructure;

namespace Marge.Core.Commands.Models
{
    class Price
    {
        private readonly State state;

        public Price(IEnumerable<IEvent> events)
        {
            state = new State(events);
        }

        public static IEnumerable<IEvent> Create(CreatePriceCommand command)
        {
            yield return new PriceCreated(command.TargetPrice, command.Cost, 0, ComputeProfit(command.TargetPrice, command.Cost));
        }

        public IEnumerable<IEvent> ChangeDiscount(ChangeDiscountCommand command)
        {
            var price = state.TargetPrice - (state.TargetPrice * 0.01m * command.Discount);
            yield return new DiscountChanged(price, command.Discount, ComputeProfit(price, state.Cost));
        }

        public IEnumerable<IEvent> Delete(DeletePriceCommand command)
        {
            yield return new PriceDeleted();
        }

        private static decimal ComputeProfit(decimal price, decimal cost)
        {
            return 1 - cost / price;
        }

        class State
        {
            public decimal Cost { get; private set; }
            public decimal TargetPrice { get; private set; }
            public bool IsDeleted { get; private set; } = false;

            public State(IEnumerable<IEvent> events)
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
