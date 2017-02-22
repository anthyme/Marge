using Marge.Common.Events;
using System.Collections.Generic;

namespace Marge.Core.Commands.Models
{
    //TODO: make private
    public class Price
    {
        public static IEnumerable<object> Create(CreatePriceCommand command)
        {
            yield return new PriceCreated(command.Price, command.Cost, 0, 1 - (command.Cost / command.Price));
        }
    }
}
