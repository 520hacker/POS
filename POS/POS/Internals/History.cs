using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace POS.Internals
{
    public class History<T> : IEnumerable<T>, IDisposable
    {
        private T[] _data = new T[10];
        private int _count;

        public void RemoveLast()
        {
            _count--;

            _data[_count - 1] = default(T);
        }

        public void Dispose()
        {
            Clear();
            _data = null;

            GC.SuppressFinalize(_data);
        }

        public void Add(T item)
        {
            if (_data.Length / 2 == _count)
            {
                var tmp = new T[_count*2];
                _data.CopyTo(tmp, 0);

                _data = tmp;
            }

            _count++;
            _data[_count] = item;
        }

        public void Clear()
        {
            _data = new T[10];
            _count = 0;
        }

        public T[] GetLastTwo()
        {
            if(_count < 2)
                return new T[0];

            return new[] { _data[_count], _data[_count - 1] };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)_data.GetEnumerator();
        }
    }
}