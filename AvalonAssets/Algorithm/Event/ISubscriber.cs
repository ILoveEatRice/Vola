namespace AvalonAssets.Algorithm.Event
{
    public interface ISubscriber
    {
    }

    public interface ISubscriber<in T> : ISubscriber
    {
        void Receive(T message);
    }
}