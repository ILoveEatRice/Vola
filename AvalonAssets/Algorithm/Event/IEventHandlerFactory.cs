namespace AvalonAssets.Algorithm.Event
{
    internal interface IEventHandlerFactory
    {
        IEventHandler Create(ISubscriber subscriber);
    }
}