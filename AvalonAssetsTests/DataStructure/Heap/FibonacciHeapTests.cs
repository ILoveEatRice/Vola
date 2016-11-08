using AvalonAssets.DataStructure.Heap;
using NUnit.Framework;

namespace AvalonAssetsTests.DataStructure.Heap
{
    [TestFixture]
    public class FibonacciHeapTests : HeapTest
    {
        public override IHeap<int> CreateHeap(bool isMin)
        {
            return new FibonacciHeap<int>(GetComparer(isMin));
        }
    }
}