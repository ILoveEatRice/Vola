using AvalonAssets.Algorithm.Event;
using AvalonAssets.Algorithm.Injection;
using AvalonAssets.Algorithm.Injection.Constructor;
using AvalonAssets.Utility;
using NUnit.Framework;

namespace AvalonAssetsTests.Algorithm.Event
{
    [TestFixture]
    public class EventAggregatorTests : ISubscriber<int>
    {
        private IContainer _container;
        private int _value;

        [OneTimeSetUp]
        public void Initialize()
        {
            _container = new Container();
            var factoryMethod = MethodUtils.GetMethodInfo(() => EventAggregators.Default(null));
            _container.RegisterType<IEventAggregator>(Constructors.StaticFactory(factoryMethod));
            _value = 0;
        }

        void ISubscriber<int>.Receive(int message)
        {
            _value = message;
        }

        [Test]
        public void Test()
        {
            var aggregator = _container.Resolve<IEventAggregator>();
            aggregator.Publish(1);
            Assert.AreEqual(0, _value);
            aggregator.Subscribe(this);
            aggregator.Publish(2);
            Assert.AreEqual(2, _value);
            aggregator.Unsubscribe(this);
            aggregator.Publish(3);
            Assert.AreEqual(2, _value);
        }
    }
}