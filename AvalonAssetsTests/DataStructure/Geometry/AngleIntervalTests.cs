using AvalonAssets.DataStructure.Geometry;
using NUnit.Framework;

namespace AvalonAssetsTests.DataStructure.Geometry
{
    [TestFixture]
    public class AngleIntervalTests
    {
        [Test]
        public void InsideTest()
        {
            Assert.Fail();
        }

        [Test]
        public void TryMergeTest()
        {
            var shadow1 = new AngleInterval(0, 30);
            var shadow2 = new AngleInterval(0, 15);
            var shadow3 = new AngleInterval(20, 45);
            var shadow4 = new AngleInterval(30, 0);
            var shadow5 = new AngleInterval(15, 5);
            AngleInterval result;

            Assert.True(shadow1.TryMerge(shadow1, out result));
            Assert.AreEqual(shadow1, result);
            Assert.True(shadow1.TryMerge(shadow1, out result));
            Assert.AreEqual(shadow1, result);

            Assert.True(shadow1.TryMerge(shadow2, out result));
            Assert.AreEqual(shadow1, result);
            Assert.True(shadow2.TryMerge(shadow1, out result));
            Assert.AreEqual(shadow1, result);

            Assert.True(shadow1.TryMerge(shadow3, out result));
            Assert.AreEqual(new AngleInterval(0, 45), result);
            Assert.True(shadow3.TryMerge(shadow1, out result));
            Assert.AreEqual(new AngleInterval(0, 45), result);

            Assert.False(shadow2.TryMerge(shadow3, out result));
            Assert.False(shadow3.TryMerge(shadow2, out result));

            Assert.True(shadow1.TryMerge(shadow4, out result));
            Assert.AreEqual(new AngleInterval(true), result);
            Assert.True(shadow4.TryMerge(shadow1, out result));
            Assert.AreEqual(new AngleInterval(true), result);

            Assert.True(shadow1.TryMerge(shadow5, out result));
            Assert.AreEqual(new AngleInterval(true), result);
            Assert.True(shadow5.TryMerge(shadow1, out result));
            Assert.AreEqual(new AngleInterval(true), result);

            Assert.True(shadow5.TryMerge(shadow4, out result));
            Assert.AreEqual(shadow5, result);
            Assert.True(shadow4.TryMerge(shadow5, out result));
            Assert.AreEqual(shadow5, result);
        }
    }
}