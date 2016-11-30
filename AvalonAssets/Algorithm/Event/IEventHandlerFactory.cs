namespace AvalonAssets.Algorithm.Event
{
    public interface IEventHandlerFactory
    {
        IEventHandler Create(ISubscriber subscriber);
    }
}