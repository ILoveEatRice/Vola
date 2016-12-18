using AvalonAssets.Algorithm.Event;

namespace AvalonAssets.Algorithm.Injection
{
    public static class Containers
    {
        public static IContainer Default()
        {
            return new Container()
                .RegisterType<IContainer, Container>()
                .RegisterType<IEventAggregator, EventAggregator>()
                .RegisterType<IEventHandlerFactory, WeakEventHandlerFactory>();
        }
    }
}