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
        private readonly IEventAggregateCommandHandler eventAggregateCommandHandler;
        private readonly List<CommandHandler> commandHandlers = new List<CommandHandler>();

        public CommandBus(IEventAggregateCommandHandler eventAggregateCommandHandler)
        {
            this.eventAggregateCommandHandler = eventAggregateCommandHandler;
        }

        public void Publish(Command command)
        {
            foreach (var commandHandler in commandHandlers)
            {
                eventAggregateCommandHandler.Handle(commandHandler, command);
            }
        }

        public void Subscribe(CommandHandler commandHandler)
        {
            commandHandlers.Add(commandHandler);
        }
    }
}
