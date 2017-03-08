using System;
using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public interface ICommandBus
    {
        void Publish(Command command);
        void Subscribe(CommandHandler commandHandler);
    }

    public delegate IEnumerable<Event> CommandHandler(IEnumerable<Event> events, Command command);

    public class CommandBus : ICommandBus
    {
        private readonly IEventStoreCommandHandler eventStoreCommandHandler;
        private readonly List<CommandHandler> commandHandlers = new List<CommandHandler>();

        public CommandBus(IEventStoreCommandHandler eventStoreCommandHandler)
        {
            this.eventStoreCommandHandler = eventStoreCommandHandler;
        }

        public void Publish(Command command)
        {
            foreach (var commandHandler in commandHandlers)
            {
                eventStoreCommandHandler.Handle(commandHandler, command);
            }
        }

        public void Subscribe(CommandHandler commandHandler)
        {
            commandHandlers.Add(commandHandler);
        }
    }
}
