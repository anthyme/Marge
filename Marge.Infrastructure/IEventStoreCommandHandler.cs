namespace Marge.Infrastructure
{
    public interface IEventStoreCommandHandler
    {
        void Handle(CommandHandler handler, Command command);
    }
}