using System;

namespace Marge.Core.Queries.Models
{
    public struct Price
    {
        public Guid Id { get; }

        public decimal Amount { get; }

        public decimal Discount { get; }

        public decimal Profit { get; }

        public Price(Guid id, decimal amount, decimal discount, decimal profit)
        {
            Id = id;
            Amount = amount;
            Discount = discount;
            Profit = profit;
        }
    }
}
