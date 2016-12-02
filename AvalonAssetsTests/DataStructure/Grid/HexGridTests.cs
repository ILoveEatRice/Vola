using System;
using System.Collections.Generic;
using AvalonAssets.Algorithm.Graph;
using AvalonAssets.Algorithm.Grid.Hex;
using AvalonAssets.DataStructure.Grid;
using NUnit.Framework;

namespace AvalonAssetsTests.DataStructure.Grid
{
    [TestFixture]
    public class HexGridTests
    {
        [SetUp]
        public void SetUp()
        {
            _grid = new HexGrid<int>();
        }

        private HexGrid<int> _grid;
        private Random _random;

        [OneTimeSetUp]
        public void Initialize()
        {
            _random = new Random();
        }

        [Test]
        public void ContainsKeyTest()
        {
            var queue = _random.UniqueNumberQueue();
            var coord = HexCoordinate.FromAxial(queue.Dequeue(), queue.Dequeue());
            _grid.Add(coord, queue.Dequeue());
            Assert.True(_grid.ContainsKey(coord));
            var coord2 = HexCoordinate.FromAxial(queue.Dequeue(), queue.Dequeue());
            Assert.False(_grid.ContainsKey(coord2));
        }

        [Test]
        public void ContainsValueTest()
        {
            var queue = _random.UniqueNumberQueue();
            var value = queue.Dequeue();
            _grid.Add(HexCoordinate.FromAxial(queue.Dequeue(), queue.Dequeue()), value);
            Assert.True(_grid.ContainsValue(value));
            Assert.False(_grid.ContainsValue(queue.Dequeue()));
        }

        [Test]
        public void GetEnumeratorTest()
        {
            var queue = _random.UniqueNumberQueue(50);
            var pairList = new List<KeyValuePair<HexCoordinate, int>>();
            for (var i = 0; i < 10; i++)
            {
                var coord = HexCoordinate.FromAxial(queue.Dequeue(), queue.Dequeue());
                var value = queue.Dequeue();
                pairList.Add(new KeyValuePair<HexCoordinate, int>(coord, value));
                _grid.Add(coord, value);
            }
            CollectionAssert.AreEquivalent(pairList, _grid);
        }

        [Test]
        public void HexGridTest()
        {
            _grid.Add(HexCoordinate.FromCube(0, 0, 0), 0);
            _grid.Add(HexCoordinate.FromCube(1, -1, 0), 0);
            _grid.Add(HexCoordinate.FromCube(1, -2, 1), 0);
            _grid.Add(HexCoordinate.FromCube(-1, 0, 1), 0);
            _grid.Add(HexCoordinate.FromCube(-1, 1, 0), 0);
            _grid.Add(HexCoordinate.FromCube(0, 1, -1), 0);
            _grid.Add(HexCoordinate.FromCube(1, 0, -1), 0);
            _grid.Add(HexCoordinate.FromCube(2, -3, 1), 0);
            var path = _grid.BreadthFirstSearch(HexCoordinate.FromCube(0, 0, 0), HexCoordinate.FromCube(2, -3, 1));
            var expected = new List<HexCoordinate>
            {
                HexCoordinate.FromCube(0, 0, 0),
                HexCoordinate.FromCube(1, -1, 0),
                HexCoordinate.FromCube(1, -2, 1),
                HexCoordinate.FromCube(2, -3, 1)
            };
            CollectionAssert.AreEquivalent(expected, path);
        }

        [Test]
        public void TryGetValueTest()
        {
            var queue = _random.UniqueNumberQueue();
            var coord = HexCoordinate.FromAxial(queue.Dequeue(), queue.Dequeue());
            var expect = queue.Dequeue();
            _grid.Add(coord, expect);
            int value;
            Assert.True(_grid.TryGetValue(coord, out value));
            Assert.AreEqual(expect, value);
            var coord2 = HexCoordinate.FromAxial(queue.Dequeue(), queue.Dequeue());
            Assert.False(_grid.TryGetValue(coord2, out value));
        }
    }
}