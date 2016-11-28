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
                var queue = containter.Register<PriorityQueue<int>, PriorityQueue<int>>()
                    .Register<IHeap<IPriority<int>>, BinaryHeap<IPriority<int>>>()
                    .Resolve<PriorityQueue<int>>();
            });
        }
    }
}