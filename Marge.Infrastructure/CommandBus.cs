using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Marge.Infrastructure
{
    public interface ICommandBus
    {
        void Publish(object command);
        ICommandBus On<TCommand>(CommandHandler<TCommand> handler);
    }

    public class CommandBus : ICommandBus, IDisposable
    {
        private readonly List<IDisposable> subscriptions = new List<IDisposable>();
        private readonly IEventAggregateCommandHandler aggregateCommandHandler;
        private readonly Subject<object> subject = new Subject<object>();

        public CommandBus(IEventAggregateCommandHandler aggregateCommandHandler)
        {
            this.aggregateCommandHandler = aggregateCommandHandler;
        }

        public void Publish(object command)
        {
            subject.OnNext(command);
        }

        public ICommandBus On<TCommand>(CommandHandler<TCommand> handler)
        {
            subscriptions.Add(subject.OfType<TCommand>().Subscribe(command => aggregateCommandHandler.Handle(handler, command)));
            return this;
        }

        public void Dispose()
        {
            subscriptions.ForEach(x => x.Dispose());
            subject?.Dispose();
        }
    }
}
