using AvalonAssets.Utility;
using NUnit.Framework;

namespace AvalonAssetsTests.Utility
{
    [TestFixture]
    public class MethodUtilsTests
    {
        [Test]
        public void GetMethodInfoTest()
        {
            var methodInfo = MethodUtils.GetMethodInfo<MethodUtilsTests>(c => c.GetMethodInfoTest());
            Assert.AreEqual("GetMethodInfoTest", methodInfo.Name);
        }
    }
}