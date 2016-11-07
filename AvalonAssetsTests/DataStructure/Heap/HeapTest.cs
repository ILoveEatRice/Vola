using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.DataStructure.Heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvalonAssetsTests.DataStructure.Heap
{
    [TestClass]
    public abstract class HeapTest
    {
        public const int Range = 100;
        private readonly Random _random = new Random();
        protected readonly List<int> TestList = new List<int>();
        protected IHeap<int> MaxHeap;
        protected IHeap<int> MinHeap;

        public abstract IHeap<int> CreateHeap(bool isMin);

        protected int RandomNumber()
        {
            return _random.Next(-Range, Range);
        }

        protected IEnumerable<int> RandomList()
        {
            var total = _random.Next(5, 20);
            for (var i = 0; i < total; i++)
                yield return RandomNumber();
        }

        [TestInitialize]
        public void Initialize()
        {
            foreach (var num in RandomList())
                TestList.Add(num);
            MinHeap = CreateHeap(true);
            foreach (var element in TestList)
                MinHeap.Insert(element);
            MaxHeap = CreateHeap(false);
            foreach (var element in TestList)
                MaxHeap.Insert(element);
        }

        [TestCleanup]
        public void ClearUp()
        {
            TestList.Clear();
        }

        [TestMethod]
        public void InsertTest()
        {
            InsertTest(MinHeap, true);
            InsertTest(MaxHeap, false);
        }


        [TestMethod]
        public void ExtractMinTest()
        {
            ExtractMinTest(MinHeap, true);
            ExtractMinTest(MaxHeap, false);
        }

        [TestMethod]
        public void GetMinTest()
        {
            GetMinTest(MinHeap, true);
            GetMinTest(MaxHeap, false);
        }

        [TestMethod]
        public void SizeTest()
        {
            SizeTest(MinHeap);
            SizeTest(MaxHeap);
        }

        [TestMethod]
        public void DecreaseKeyTest()
        {
            DecreaseKeyTest(MinHeap, true);
            DecreaseKeyTest(MaxHeap, false);
        }

        [TestMethod]
        public void DeleteTest()
        {
            DeleteTest(MinHeap, true);
            DeleteTest(MaxHeap, false);
        }


        public void InsertTest(IHeap<int> heap, bool isMin)
        {
            var newList = RandomList().ToList();
            var tmpLst = new List<int>(TestList);
            tmpLst.AddRange(newList);
            tmpLst.Sort();
            if (!isMin)
                tmpLst.Reverse();
            foreach (var num in newList)
                heap.Insert(num);
            var heapList = new List<int>();
            while (!heap.IsEmpty())
                heapList.Add(heap.ExtractMin().Value);
            Console.WriteLine("isMin:" + isMin);
            Console.WriteLine("Expect:" + string.Join(", ", tmpLst));
            Console.WriteLine("Result:" + string.Join(", ", heapList));
            Assert.IsTrue(tmpLst.SequenceEqual(heapList));
        }

        public void ExtractMinTest(IHeap<int> heap, bool isMin)
        {
            var tmpLst = new List<int>(TestList);
            Console.WriteLine("isMin:" + isMin);
            var expected = isMin ? tmpLst.Min() : tmpLst.Max();
            var result = heap.ExtractMin().Value;
            Console.WriteLine("Expect:" + expected + " Result:" + result);
            Assert.AreEqual(expected, result);
            expected = tmpLst.Count - 1;
            result = heap.Size();
            Console.WriteLine("Expect:" + expected + " Result:" + result);
            Assert.AreEqual(expected, result);
        }


        public void GetMinTest(IHeap<int> heap, bool isMin)
        {
            var tmpLst = new List<int>(TestList);
            Console.WriteLine("isMin:" + isMin);
            var expected = isMin ? tmpLst.Min() : tmpLst.Max();
            var result = heap.GetMin().Value;
            Console.WriteLine("Expect:" + expected + " Result:" + result);
            Assert.AreEqual(expected, result);
        }


        public void SizeTest(IHeap<int> heap)
        {
            var tmpLst = new List<int>(TestList);
            var expected = tmpLst.Count;
            var result = heap.Size();
            Console.WriteLine("Expect:" + expected + " Result:" + result);
            Assert.AreEqual(expected, result);
        }


        public void DecreaseKeyTest(IHeap<int> heap, bool isMin)
        {
            var num = RandomNumber();
            var tmpLst = new List<int>(TestList) {num};
            tmpLst.Sort();
            if (!isMin)
                tmpLst.Reverse();
            var node = heap.Insert(isMin ? num + 1 : num - 1);
            node.Value = num;
            heap.DecreaseKey(node);
            var heapList = GetHeapList(isMin).ToList();
            Console.WriteLine("isMin:" + isMin);
            Console.WriteLine("Expect:" + string.Join(", ", tmpLst));
            Console.WriteLine("Result:" + string.Join(", ", heapList));
            Assert.IsTrue(tmpLst.SequenceEqual(heapList));
        }


        public void DeleteTest(IHeap<int> heap, bool isMin)
        {
            var num = RandomNumber();
            var tmpLst = new List<int>(TestList) {num};
            tmpLst.Remove(num);
            tmpLst.Sort();
            if (!isMin)
                tmpLst.Reverse();
            var node = heap.Insert(num);
            heap.Delete(node);
            var heapList = GetHeapList(isMin).ToList();
            Console.WriteLine("isMin:" + isMin);
            Console.WriteLine("Expect:" + string.Join(", ", tmpLst));
            Console.WriteLine("Result:" + string.Join(", ", heapList));
            Assert.IsTrue(tmpLst.SequenceEqual(heapList));
        }

        protected IEnumerable<int> GetHeapList(bool isMin)
        {
            if (isMin)
                while (!MinHeap.IsEmpty())
                    yield return MinHeap.ExtractMin().Value;
            else
                while (!MaxHeap.IsEmpty())
                    yield return MaxHeap.ExtractMin().Value;
        }

        protected IComparer<int> GetComparer(bool isMin)
        {
            return isMin ? new Comparer((x, y) => x - y) : new Comparer((x, y) => y - x);
        }

        private class Comparer : IComparer<int>
        {
            private readonly Func<int, int, int> _compareFunc;

            public Comparer(Func<int, int, int> compareFunc)
            {
                _compareFunc = compareFunc;
            }

            public int Compare(int x, int y)
            {
                return _compareFunc(x, y);
            }
        }
    }
}