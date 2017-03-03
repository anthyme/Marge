using Marge.Core.Commands.Models;
using Marge.Infrastructure;

namespace Marge.Core.Commands.Handlers
{
    public static class PriceCommandHandler
    {
        public static void RegisterCommands(ICommandBus bus)
        {
            bus
            .On<CreatePriceCommand>((_, cmd) => Price.Create(cmd))
            .On<DeletePriceCommand>((events, cmd) => new Price(events).Delete(cmd))
            .On<ChangeDiscountCommand>((events, cmd) => new Price(events).ChangeDiscount(cmd));
        }
    }
}
