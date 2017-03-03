using System;
using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public interface ICommandBus
    {
        void Publish(object command);
        ICommandBus On<TCommand>(CommandHandler<TCommand> handler) where TCommand : Command;
    }

    public class CommandBus : ICommandBus
    {
        private readonly IEventAggregateCommandHandler aggregateCommandHandler;
        private readonly IDictionary<Type, Action<object>> subscritions = new Dictionary<Type, Action<object>>();

        public CommandBus(IEventAggregateCommandHandler aggregateCommandHandler)
        {
            this.aggregateCommandHandler = aggregateCommandHandler;
        }

        public void Publish(object command)
        {
            subscritions[command.GetType()](command);
        }

        public ICommandBus On<TCommand>(CommandHandler<TCommand> handler) where TCommand : Command
        {
            subscritions[typeof(TCommand)] = x => aggregateCommandHandler.Handle(handler, (TCommand)x);
            return this;
        }
    }
}
