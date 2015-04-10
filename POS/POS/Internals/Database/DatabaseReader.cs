using System;
using System.IO;
using Polenter.Serialization;

namespace POS.Internals.Database
{
    public class DatabaseReader
    {
        public static T Read<T>(string uri)
        {
            var s = new SharpSerializer();
            return (T)s.Deserialize(uri);
        }

        public static T Read<T>(Stream strm)
        {
            var s = new SharpSerializer();
            return (T)s.Deserialize(strm);
        }
    }
}