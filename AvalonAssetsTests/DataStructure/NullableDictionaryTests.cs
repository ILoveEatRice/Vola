using System;
using System.Collections.Generic;
using AvalonAssets.DataStructure;
using NUnit.Framework;

namespace AvalonAssetsTests.DataStructure
{
    [TestFixture]
    public class NullableDictionaryTests
    {
       [TearDown]
        public void ClearUp()
        {
            _nullableDictionary.Clear();
            _dictionary.Clear();
        }

        public const int Range = 100;
        private readonly Random _random = new Random();
        private NullableDictionary<string, int> _nullableDictionary;
        private Dictionary<string, int> _dictionary;

        [OneTimeSetUp]
        public void Setup()
        {
            _nullableDictionary = new NullableDictionary<string, int>();
            _dictionary = new Dictionary<string, int>();
        }

        protected int RandomNumber()
        {
            return _random.Next(-Range, Range);
        }

        [Test]
        public void AddTest()
        {
            foreach (var num in _random.UniqueNumberList())
            {
                _nullableDictionary.Add(num.ToString(), num);
                _dictionary.Add(num.ToString(), num);
            }
            CollectionAssert.AreEquivalent(_dictionary, _nullableDictionary);
        }

        [Test]
        public void ClearTest()
        {
            _nullableDictionary.Add("1", 1);
            _nullableDictionary.Add(null, 1);
            Assert.True(_nullableDictionary.ContainsKey("1"));
            Assert.True(_nullableDictionary.ContainsKey(null));
            _nullableDictionary.Clear();
            Assert.False(_nullableDictionary.ContainsKey("1"));
            Assert.False(_nullableDictionary.ContainsKey(null));
        }

        [Test]
        public void ContainsKeyTest()
        {
            _nullableDictionary.Add("1", RandomNumber());
            _nullableDictionary.Add(null, RandomNumber());
            Assert.True(_nullableDictionary.ContainsKey("1"));
            Assert.True(_nullableDictionary.ContainsKey(null));
            Assert.False(_nullableDictionary.ContainsKey("2"));
        }

        [Test]
        public void RemoveTest()
        {
            _nullableDictionary.Add("1", RandomNumber());
            Assert.True(_nullableDictionary.ContainsKey("1"));
            _nullableDictionary.Remove("1");
            Assert.False(_nullableDictionary.ContainsKey("1"));
        }

        [Test]
        public void TryGetValueTest()
        {
            var expected = RandomNumber();
            int result;
            Assert.False(_nullableDictionary.TryGetValue("1", out result));
            _nullableDictionary.Add("1", expected);
            Assert.True(_nullableDictionary.TryGetValue("1", out result));
            Assert.AreEqual(expected, result);
        }
    }
}