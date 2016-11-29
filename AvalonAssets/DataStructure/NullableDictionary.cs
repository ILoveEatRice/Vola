using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace AvalonAssets.DataStructure
{
    public class NullableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary where TKey : class
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        private bool _hasNullValue;
        private TValue _nullValue;

        public NullableDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public NullableDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        ICollection IDictionary.Values
        {
            get
            {
                var collection = ((IDictionary) _dictionary).Values;
                if (!_hasNullValue) return collection;
                var array = new TValue[collection.Count + 1];
                array[0] = _nullValue;
                collection.CopyTo(array, 1);
                return array;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {

                var collection = ((IDictionary)_dictionary).Values;
                if (!_hasNullValue) return collection;
                var array = new TKey[collection.Count + 1];
                array[0] = null;
                collection.CopyTo(array, 1);
                return array;
            }
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        object IDictionary.this[object key]
        {
            get { return this[(TKey) key]; }
            set { this[(TKey) key] = (TValue) value; }
        }

        public bool Contains(object key)
        {
            throw new NotImplementedException();
        }

        public void Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (_hasNullValue)
                array.SetValue(new KeyValuePair<TKey, TValue>(null, _nullValue), index++);
            ICollection dictionary = _dictionary;
            dictionary.CopyTo(array, index);
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection) _dictionary).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection) _dictionary).IsSynchronized; }
        }

        bool IDictionary.IsFixedSize
        {
            get { return ((IDictionary) _dictionary).IsFixedSize; }
        }

        public bool ContainsKey(TKey key)
        {
            return key == null ? _hasNullValue : _dictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                if (_hasNullValue)
                    throw new ArgumentException("Item with Same Key has already been added.");
                _nullValue = value;
                _hasNullValue = true;
            }
            else
                _dictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            if (key != null) return _dictionary.Remove(key);
            if (!_hasNullValue) return false;
            _nullValue = default(TValue);
            _hasNullValue = false;
            return true;
        }

        public bool TryGetValue([CanBeNull] TKey key, out TValue value)
        {
            if (key != null) return _dictionary.TryGetValue(key, out value);
            if (_hasNullValue)
            {
                value = _nullValue;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get { return key == null ? _nullValue : _dictionary[key]; }
            set
            {
                if (key == null)
                {
                    _nullValue = value;
                    _hasNullValue = true;
                }
                else
                    _dictionary[key] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                if (!_hasNullValue) return _dictionary.Keys;
                return new List<TKey>(_dictionary.Keys) {null};
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                if (!_hasNullValue) return _dictionary.Values;
                return new List<TValue>(_dictionary.Values) {_nullValue};
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (_hasNullValue)
                yield return new KeyValuePair<TKey, TValue>(null, _nullValue);
            foreach (var keyValuePair in _dictionary)
                yield return keyValuePair;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
            {
                _nullValue = item.Value;
                _hasNullValue = true;
            }
            else
            {
                ICollection<KeyValuePair<TKey, TValue>> dictionary = _dictionary;
                dictionary.Add(item);
            }
        }

        public void Clear()
        {
            _dictionary.Clear();
            _nullValue = default(TValue);
            _hasNullValue = false;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            ICollection<KeyValuePair<TKey, TValue>> dictionary = _dictionary;
            return item.Key == null ? _hasNullValue : dictionary.Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ICollection<KeyValuePair<TKey, TValue>> dictionary = _dictionary;
            if (_hasNullValue)
                array[arrayIndex++] = new KeyValuePair<TKey, TValue>(null, _nullValue);
            dictionary.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
                return Remove(null);
            ICollection<KeyValuePair<TKey, TValue>> dictionary = _dictionary;
            return dictionary.Remove(item);
        }

        public int Count
        {
            get { return _hasNullValue ? _dictionary.Count + 1 : _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}