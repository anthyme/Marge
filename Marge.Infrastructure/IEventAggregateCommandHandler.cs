namespace Marge.Infrastructure
{
    public interface IEventAggregateCommandHandler
    {
        void Handle(CommandHandler handler, Command command);
    }
}