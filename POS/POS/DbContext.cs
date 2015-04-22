using Creek.Database.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POS
{
    public class DbContext
    {
        private static IOdb db;

        public static void Open(string filename)
        {
            db = Creek.Database.OdbFactory.Open(filename);
        }

        public static void Add(object obj)
        {
            db.Store(obj);
        }

        public static IEnumerable<T> GetItems<T>()
            where T : new()
        {
            return db.QueryAndExecute<T>();
        }

        public static void Delete(object obj)
        {
            db.Delete(obj);
        }

        public static void Close()
        {
            if(!db.IsClosed())
                db.Close();
        }
    }
}