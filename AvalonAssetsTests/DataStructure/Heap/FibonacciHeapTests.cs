using AvalonAssets.DataStructure.Heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvalonAssetsTests.DataStructure.Heap
{
    [TestClass]
    public class FibonacciHeapTests : HeapTest
    {
        public override IHeap<int> CreateHeap(bool isMin)
        {
            return new FibonacciHeap<int>(GetComparer(isMin));
        }
    }
}