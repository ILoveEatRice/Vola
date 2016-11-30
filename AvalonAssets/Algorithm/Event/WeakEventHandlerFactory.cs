namespace AvalonAssets.Algorithm.Event
{
    /// <summary>
    ///     <see cref="IEventHandlerFactory" /> creates <see cref="WeakEventHandler" />.
    /// </summary>
    public class WeakEventHandlerFactory : IEventHandlerFactory
    {
        public IEventHandler Create(ISubscriber subscriber)
        {
            return new WeakEventHandler(subscriber);
        }
    }
}