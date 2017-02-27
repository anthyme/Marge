using System;
using Marge.Infrastructure;

namespace Marge.Core.Queries.Models
{
    public class Price : Value
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

        protected override object ValueSignature => new { Id, Amount, Discount, Profit };
    }
}
