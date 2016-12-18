using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Algorithm.Injection;
using AvalonAssets.Algorithm.Injection.Parameter;
using AvalonAssets.DataStructure.Heap;
using NUnit.Framework;

namespace AvalonAssetsTests.Algorithm.Injection
{
    [TestFixture]
    public class ContainerTests
    {
        [SetUp]
        public void Initialize()
        {
            _container = new Container();
        }

        private IContainer _container;

        [Test]
        public void IsRegisteredTest()
        {
            Assert.False(_container.IsRegistered<IHeap<int>>());
            _container.RegisterType<IHeap<int>, BinomialHeap<int>>();
            Assert.True(_container.IsRegistered<IHeap<int>>());
            _container.RegisterType<IHeap<int>, BinaryHeap<int>>()
                .RegisterInstance<IComparer<int>>(Comparer<int>.Default);
            _container.Resolve<IHeap<int>>(Parameters.Default("elements"));
        }

        [Test]
        public void RegisterInstanceTest()
        {
            _container.RegisterInstance<IHeap<int>>(new BinomialHeap<int>(Comparer<int>.Default));
            Assert.AreSame(_container.Resolve<IHeap<int>>(), _container.Resolve<IHeap<int>>());
        }

        [Test]
        public void RegisterTypeTest()
        {
            _container.RegisterInstance<IComparer<int>>(Comparer<int>.Default);
            _container.RegisterType<IHeap<int>, BinomialHeap<int>>();
            Assert.True(_container.Resolve<IHeap<int>>() is BinomialHeap<int>);
        }

        [Test]
        public void ResolveAllTest()
        {
            _container.RegisterInstance<IComparer<int>>(Comparer<int>.Default);
            _container.RegisterType<IHeap<int>, BinomialHeap<int>>();
            _container.RegisterType<IHeap<int>, BinaryHeap<int>>("2");
            var excepted = new List<Type>
            {
                typeof(BinomialHeap<int>),
                typeof(BinaryHeap<int>)
            };
            var result = _container.ResolveAll<IHeap<int>>().Select(h => h.GetType()).ToList();
            CollectionAssert.AreEquivalent(excepted, result);
        }
    }
}