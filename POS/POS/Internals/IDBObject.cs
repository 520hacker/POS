using System;
using System.Linq;

namespace POS.Internals
{
    public interface IDBObject<T>
    {
        T From(dynamic d);
    }
}