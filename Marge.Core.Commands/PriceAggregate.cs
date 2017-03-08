using Marge.Common.Events;
using System.Collections.Generic;
using Marge.Infrastructure;

namespace Marge.Core.Commands
{
    public static class PriceAggregate
    {
        public static IEnumerable<Event> Handle(IEnumerable<Event> events, Command command)
        {
            var state = new State(events);
            switch (command)
            {
                case CreatePriceCommand x: return CreatePrice(x);
                case ChangeDiscountCommand x: return ChangeDiscount(x);
                case DeletePriceCommand x: return new [] { new PriceDeleted() };
                default: return new Event[0];
            }

            IEnumerable<Event> CreatePrice(CreatePriceCommand cmd)
            {
                yield return new PriceCreated(cmd.TargetPrice, cmd.Cost, 0, ComputeProfit(cmd.TargetPrice, cmd.Cost));
            }

            IEnumerable<Event> ChangeDiscount(ChangeDiscountCommand cmd)
            {
                if (state.IsDeleted) yield break;
                var price = state.TargetPrice - (state.TargetPrice * 0.01m * cmd.Discount);
                yield return new DiscountChanged(price, cmd.Discount, ComputeProfit(price, state.Cost));
            }

            decimal ComputeProfit(decimal price, decimal cost) => 1 - cost / price;
        }

        class State
        {
            public decimal Cost { get; private set; }
            public decimal TargetPrice { get; private set; }
            public bool IsDeleted { get; private set; }

            public State(IEnumerable<Event> events)
            {
                foreach (var evt in events)
                {
                    switch (evt)
                    {
                        case PriceCreated x: Apply(x); break;
                        case PriceDeleted x: Apply(x); break;
                    }
                }
            }

            private void Apply(PriceCreated evt)
            {
                Cost = evt.Cost;
                TargetPrice = evt.Price;
            }

            private void Apply(PriceDeleted _) => IsDeleted = true;
        }
    }
}
