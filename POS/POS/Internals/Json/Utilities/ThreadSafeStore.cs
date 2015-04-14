using System;
using System.Collections.Generic;

namespace Lib.JSON.Utilities
{
    internal class ThreadSafeStore<TKey, TValue>
    {
        private readonly object _lock = new object();
        private readonly Func<TKey, TValue> _creator;

        private Dictionary<TKey, TValue> _store;

        public ThreadSafeStore(Func<TKey, TValue> creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException("creator");
            }

            this._creator = creator;
        }

        public TValue Get(TKey key)
        {
            if (this._store == null)
            {
                return this.AddValue(key);
            }

            TValue value;
            if (!this._store.TryGetValue(key, out value))
            {
                return this.AddValue(key);
            }

            return value;
        }

        private TValue AddValue(TKey key)
        {
            TValue value = this._creator(key);

            lock (this._lock)
            {
                if (this._store == null)
                {
                    this._store = new Dictionary<TKey, TValue>();
                    this._store[key] = value;
                }
                else
                {
                    // double check locking
                    TValue checkValue;
                    if (this._store.TryGetValue(key, out checkValue))
                    {
                        return checkValue;
                    }

                    Dictionary<TKey, TValue> newStore = new Dictionary<TKey, TValue>(this._store);
                    newStore[key] = value;

                    this._store = newStore;
                }

                return value;
            }
        }
    }
}