using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lib.JSON.Utilities
{
    internal class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, IWrappedDictionary
    {
        private readonly IDictionary _dictionary;
        private readonly IDictionary<TKey, TValue> _genericDictionary;
        private object _syncRoot;

        public DictionaryWrapper(IDictionary dictionary)
        {
            ValidationUtils.ArgumentNotNull(dictionary, "dictionary");

            this._dictionary = dictionary;
        }

        public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
        {
            ValidationUtils.ArgumentNotNull(dictionary, "dictionary");

            this._genericDictionary = dictionary;
        }

        public ICollection<TKey> Keys
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return this._genericDictionary.Keys;
                }
                else
                {
                    return this._dictionary.Keys.Cast<TKey>().ToList();
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return this._genericDictionary.Values;
                }
                else
                {
                    return this._dictionary.Values.Cast<TValue>().ToList();
                }
            }
        }

        public int Count
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return this._genericDictionary.Count;
                }
                else
                {
                    return this._dictionary.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return this._genericDictionary.IsReadOnly;
                }
                else
                {
                    return this._dictionary.IsReadOnly;
                }
            }
        }

        public object UnderlyingDictionary
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return this._genericDictionary;
                }
                else
                {
                    return this._dictionary;
                }
            }
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return false;
                }
                else
                {
                    return this._dictionary.IsFixedSize;
                }
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return this._genericDictionary.Keys.ToList();
                }
                else
                {
                    return this._dictionary.Keys;
                }
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return this._genericDictionary.Values.ToList();
                }
                else
                {
                    return this._dictionary.Values;
                }
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return false;
                }
                else
                {
                    return this._dictionary.IsSynchronized;
                }
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                }

                return this._syncRoot;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return this._genericDictionary[key];
                }
                else
                {
                    return (TValue)this._dictionary[key];
                }
            }
            set
            {
                if (this._genericDictionary != null)
                {
                    this._genericDictionary[key] = value;
                }
                else
                {
                    this._dictionary[key] = value;
                }
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                if (this._genericDictionary != null)
                {
                    return this._genericDictionary[(TKey)key];
                }
                else
                {
                    return this._dictionary[key];
                }
            }
            set
            {
                if (this._genericDictionary != null)
                {
                    this._genericDictionary[(TKey)key] = (TValue)value;
                }
                else
                {
                    this._dictionary[key] = value;
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.Add(key, value);
            }
            else
            {
                this._dictionary.Add(key, value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            if (this._genericDictionary != null)
            {
                return this._genericDictionary.ContainsKey(key);
            }
            else
            {
                return this._dictionary.Contains(key);
            }
        }

        public bool Remove(TKey key)
        {
            if (this._genericDictionary != null)
            {
                return this._genericDictionary.Remove(key);
            }
            else
            {
                if (this._dictionary.Contains(key))
                {
                    this._dictionary.Remove(key);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this._genericDictionary != null)
            {
                return this._genericDictionary.TryGetValue(key, out value);
            }
            else
            {
                if (!this._dictionary.Contains(key))
                {
                    value = default(TValue);
                    return false;
                }
                else
                {
                    value = (TValue)this._dictionary[key];
                    return true;
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.Add(item);
            }
            else
            {
                ((IList)this._dictionary).Add(item);
            }
        }

        public void Clear()
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.Clear();
            }
            else
            {
                this._dictionary.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (this._genericDictionary != null)
            {
                return this._genericDictionary.Contains(item);
            }
            else
            {
                return ((IList)this._dictionary).Contains(item);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.CopyTo(array, arrayIndex);
            }
            else
            {
                foreach (DictionaryEntry item in this._dictionary)
                {
                    array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)item.Key, (TValue)item.Value);
                }
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (this._genericDictionary != null)
            {
                return this._genericDictionary.Remove(item);
            }
            else
            {
                if (this._dictionary.Contains(item.Key))
                {
                    object value = this._dictionary[item.Key];

                    if (object.Equals(value, item.Value))
                    {
                        this._dictionary.Remove(item.Key);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (this._genericDictionary != null)
            {
                return this._genericDictionary.GetEnumerator();
            }
            else
            {
                return this._dictionary.Cast<DictionaryEntry>().Select(de => new KeyValuePair<TKey, TValue>((TKey)de.Key, (TValue)de.Value)).GetEnumerator();
            }
        }

        public void Remove(object key)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.Remove((TKey)key);
            }
            else
            {
                this._dictionary.Remove(key);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void IDictionary.Add(object key, object value)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.Add((TKey)key, (TValue)value);
            }
            else
            {
                this._dictionary.Add(key, value);
            }
        }

        bool IDictionary.Contains(object key)
        {
            if (this._genericDictionary != null)
            {
                return this._genericDictionary.ContainsKey((TKey)key);
            }
            else
            {
                return this._dictionary.Contains(key);
            }
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            if (this._genericDictionary != null)
            {
                return new DictionaryEnumerator<TKey, TValue>(this._genericDictionary.GetEnumerator());
            }
            else
            {
                return this._dictionary.GetEnumerator();
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (this._genericDictionary != null)
            {
                this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
            }
            else
            {
                this._dictionary.CopyTo(array, index);
            }
        }

        private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IDictionaryEnumerator
        {
            private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

            public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
            {
                ValidationUtils.ArgumentNotNull(e, "e");
                this._e = e;
            }

            public DictionaryEntry Entry
            {
                get
                {
                    return (DictionaryEntry)this.Current;
                }
            }

            public object Key
            {
                get
                {
                    return this.Entry.Key;
                }
            }

            public object Value
            {
                get
                {
                    return this.Entry.Value;
                }
            }

            public object Current
            {
                get
                {
                    return new DictionaryEntry(this._e.Current.Key, this._e.Current.Value);
                }
            }

            public bool MoveNext()
            {
                return this._e.MoveNext();
            }

            public void Reset()
            {
                this._e.Reset();
            }
        }
    }
}
