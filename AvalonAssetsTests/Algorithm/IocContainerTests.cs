using AvalonAssets.Algorithm;
using AvalonAssets.DataStructure.Heap;
using AvalonAssets.DataStructure.Queue;
using NUnit.Framework;

namespace AvalonAssetsTests.Algorithm
{
    [TestFixture]
    public class IocContainerTests
    {
        [Test]
        public void ResolveTest()
        {
            Assert.DoesNotThrow(() =>
            {
                var containter = new IocContainer();
                containter.Register<PriorityQueue<int>, PriorityQueue<int>>();
                containter.Register<IHeap<IPriority<int>>, BinaryHeap<IPriority<int>>>();
                var queue = containter.Resolve<PriorityQueue<int>>();
            });
        }
    }
}