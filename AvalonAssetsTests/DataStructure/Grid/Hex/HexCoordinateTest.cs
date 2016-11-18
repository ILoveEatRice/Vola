using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Grid.Hex;
using AvalonAssets.Utility;
using NUnit.Framework;

namespace AvalonAssetsTests.DataStructure.Grid.Hex
{
    [TestFixture]
    public class HexCoordinateTest
    {
        protected HexCoordinate Center;

        [OneTimeSetUp]
        public void Initialize()
        {
            Center = HexCoordinate.FromCube(1, 1, -2);
        }

        [Test]
        public void DiagonalTest()
        {
            var expected = new List<HexCoordinate>
            {
                HexCoordinate.FromCube(3, 0, -3),
                HexCoordinate.FromCube(2, -1, -1),
                HexCoordinate.FromCube(0, 0, 0),
                HexCoordinate.FromCube(-1, 2, -1),
                HexCoordinate.FromCube(0, 3, -3),
                HexCoordinate.FromCube(2, 2, -4)
            };
            var result =
                EnumUtils.Values<HexCoordinate.Direction>().Select(direction => Center.Diagonal(direction)).ToList();
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void DistanceTest()
        {
            Assert.AreEqual(2, Center.Distance(HexCoordinate.FromCube(1, -1, 0)));
        }

        [Test]
        public void FieldOfViewTest()
        {
            Func<HexCoordinate, bool> isBlock = c =>
                c.Equals(HexCoordinate.FromCube(0, 0, 0)) ||
                c.Equals(HexCoordinate.FromCube(1, 0, -1)) ||
                c.Equals(HexCoordinate.FromCube(2, 0, -2)) ||
                c.Equals(HexCoordinate.FromCube(0, 2, -2));
            var field = Center.FieldOfView(5, isBlock).ToList();
            Assert.False(field.Contains(HexCoordinate.FromCube(0, 0, 0)));
            Assert.False(field.Contains(HexCoordinate.FromCube(-1, -2, 3)));
            Assert.False(field.Contains(HexCoordinate.FromCube(2, -1, -1)));
            Assert.False(field.Contains(HexCoordinate.FromCube(3, 0, -3)));
            Assert.True(field.Contains(HexCoordinate.FromCube(1, 2, -3)));
            Assert.True(field.Contains(HexCoordinate.FromCube(4, 0, -4)));
        }

        [Test]
        public void FromCubeTest()
        {
            Assert.Throws<ArgumentException>(() => { HexCoordinate.FromCube(1, 1, 1); });
        }

        [Test]
        public void FromRingTest()
        {
            Assert.AreEqual(HexCoordinate.FromCube(1, -1, 0), Center.FromRing(2, 5));
            Assert.AreEqual(HexCoordinate.FromCube(4, -2, -2), Center.FromRing(3, 4));
        }

        [Test]
        public void LineTest()
        {
            var expected = new List<HexCoordinate>
            {
                Center,
                HexCoordinate.FromCube(1, 0, -1),
                HexCoordinate.FromCube(0, 0, 0),
                HexCoordinate.FromCube(0, -1, 1)
            };
            CollectionAssert.AreEqual(expected, Center.Line(HexCoordinate.FromCube(0, -1, 1)));
        }

        [Test]
        public void NeighborTest()
        {
            Assert.AreEqual(HexCoordinate.FromCube(2, 0, -2), Center.Neighbor(HexCoordinate.Direction.A));
            var neighbor =
                EnumUtils.Values<HexCoordinate.Direction>().Select(direction => Center.Neighbor(direction)).ToList();
            CollectionAssert.AreEquivalent(neighbor, Center.AllNeighbors());
            neighbor = new List<HexCoordinate>
            {
                HexCoordinate.FromCube(2, 0, -2),
                HexCoordinate.FromCube(1, 0, -1),
                HexCoordinate.FromCube(0, 1, -1),
                HexCoordinate.FromCube(0, 2, -2),
                HexCoordinate.FromCube(1, 2, -3),
                HexCoordinate.FromCube(2, 1, -3)
            };
            CollectionAssert.AreEquivalent(neighbor, Center.AllNeighbors());
        }

        [Test]
        public void RangeTest()
        {
            var expected = Center.AllNeighbors().ToList();
            expected.Add(Center);
            CollectionAssert.AreEquivalent(expected, Center.Range(1));
            CollectionAssert.AreEquivalent(new List<HexCoordinate> {Center}, Center.Range(0));
        }

        [Test]
        public void ReachableTest()
        {
            var expected = new List<HexCoordinate>
            {
                HexCoordinate.FromCube(0, 1, -1),
                HexCoordinate.FromCube(0, 2, -2),
                HexCoordinate.FromCube(1, 2, -3),
                HexCoordinate.FromCube(2, 1, -3)
            };
            Func<HexCoordinate, bool> isBlock = c =>
                c.Equals(HexCoordinate.FromCube(0, 0, 0)) ||
                c.Equals(HexCoordinate.FromCube(1, 0, -1)) ||
                c.Equals(HexCoordinate.FromCube(2, 0, -2));
            CollectionAssert.AreEquivalent(expected, Center.Reachable(1, isBlock));
        }


        [Test]
        public void RingOfTest()
        {
            Assert.AreEqual(new RingCoordinate(Center, 3, 9), Center.RingOf(HexCoordinate.FromCube(-1, 0, 1)));
            Assert.AreEqual(Center.FromRing(3, 9), Center.RingOf(HexCoordinate.FromCube(-1, 0, 1)));
            Assert.AreEqual(Center.FromRing(2, 3), Center.RingOf(Center.FromRing(2, 3)));
        }

        [Test]
        public void RingTest()
        {
            var expected = Center.Range(1).ToList();
            expected.Remove(Center);
            CollectionAssert.AreEquivalent(expected, Center.Ring(1));
            CollectionAssert.AreEquivalent(Center.AllNeighbors(), Center.Ring(1));
            const int size = 3;
            var result = new List<HexCoordinate>();
            for (var i = 0; i <= size; i++)
                result.AddRange(Center.Ring(i));
            CollectionAssert.AreEquivalent(Center.Range(size), result);
        }

        [Test]
        public void RotateTest()
        {
            Assert.AreEqual(HexCoordinate.FromCube(-2, 4, -2), Center.Rotate(HexCoordinate.FromCube(4, -2, -2), 3));
        }

        [Test]
        public void SpiralTest()
        {
            const int size = 3;
            var expected = new List<HexCoordinate>();
            for (var i = 0; i <= size; i++)
                expected.AddRange(Center.Ring(i));
            CollectionAssert.AreEqual(expected, Center.Spiral(size));
        }

        [Test]
        public void VisibleTest()
        {
            Func<HexCoordinate, bool> isBlock = c => c.Equals(HexCoordinate.FromCube(0, 0, 0));
            Assert.True(Center.Visible(HexCoordinate.FromCube(1, -1, 0), isBlock));
            Assert.False(Center.Visible(HexCoordinate.FromCube(-1, 0, 1), isBlock));
        }
    }
}