using System;
using System.Collections.Generic;
using System.Linq;
using POS.Internals;

namespace POS
{
    public class DbContext
    {
        public static string AddTable<T>()
        {
            var t = typeof(T);

            string sqlsc = string.Format("CREATE TABLE {0}s (", t.Name);
            foreach (var p in t.GetProperties())
            {
                sqlsc += string.Format("\n {0} ", p.Name);
                if (p.PropertyType.ToString().Contains("System.Int32"))
                {
                    sqlsc += " int ";
                }
                else if (p.PropertyType.ToString().Contains("System.DateTime"))
                {
                    sqlsc += " datetime ";
                }
                else
                {
                    sqlsc += " varchar(255) ";
                }

                sqlsc += ",";
            }
            return string.Format("{0})", sqlsc.Substring(0, sqlsc.Length - 1));
        }

        public static void Add<T>(T obj)
        {
            SqlHelper.query(string.Format("INSERT INTO {0}s VALUES ()", typeof(T).Name));
        }

        public static IEnumerable<T> GetItems<T>()
            where T : IDBObject<T>, new()
        {
            var q = SqlHelper.query(string.Format("SELECT * FROM {0}s", typeof(T).Name));
            var arr = SqlHelper.fetch_array(q);

            var ret = new List<T>();

            foreach (var p in arr)
            {
                var tmp = new T();
                   
                ret.Add(tmp.From(p));
            }

            return ret;
        }

        public static string Insert(object obj)
        {
            return null;
        }
    }
}