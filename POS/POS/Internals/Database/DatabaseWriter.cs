using System;
using System.IO;
using Polenter.Serialization;

namespace POS.Internals.Database
{
    public class DatabaseWriter
    {
        public static void Write<T>(string uri, T obj)
        {
            var s = new SharpSerializer();
            s.Serialize(obj, uri);
        }

        public static void Write<T>(Stream strm, T obj)
        {
            var s = new SharpSerializer();
            s.Serialize(obj, strm);
        }
    }
}