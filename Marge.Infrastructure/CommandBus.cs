using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Marge.Infrastructure
{
    public interface ICommandBus
    {
        void Publish(object command);
        IDisposable Subscribe<TCommand>(IHandle<TCommand> handler);
    }

    public class CommandBus : ICommandBus
    {
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

        public IDisposable Subscribe<TCommand>(IHandle<TCommand> handler)
        {
            return subject.OfType<TCommand>().Subscribe(command => aggregateCommandHandler.Handle(handler, command));
        }
    }
}
