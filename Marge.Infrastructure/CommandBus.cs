using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Marge.Infrastructure
{
    public class CommandBus
    {
        public static CommandBus Instance => new CommandBus();

        private Subject<object> subject = new Subject<object>();

        public void Publish(object command)
        {
            subject.OnNext(command);
        }

        public IDisposable Subscribe<T>(IHandle<T> handler)
        {
            return subject.OfType<T>().Subscribe(x => handler.Handle(x));
        }
    }
}
