using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using SharpHsql;

namespace POS.Internals
{
    public static class SqlHelper
    {
        private static SharpHsql.Database db;
        private static Channel myChannel;

        private static string errormsg;

        public static void auto_commit(bool b)
        {
            myChannel.SetAutoCommit(b);
        }

        public static void close()
        {
            db.Execute("shutdown", myChannel);
            myChannel.Disconnect();
        }

        public static void commit()
        {
            myChannel.Commit();
        }

        public static void connect(string username, string password)
        {
            myChannel = db.Connect(username, password);
        }

        public static string error()
        {
            return errormsg;
        }

        public static string escape_string(string s)
        {
            return Regex.Escape(s);
        }

        public static List<Dictionary<string, object>> fetch_array(Result rs)
        {
            List<Dictionary<string, object>> dictionaries = new List<Dictionary<string, object>>();
            if (rs.Root != null)
            {
                for (Record i = rs.Root; i != null; i = i.Next)
                {
                    Dictionary<string, object> strs = new Dictionary<string, object>();
                    for (int j = 0; j < (int)rs.Label.Length; j++)
                    {
                        string str = readableString(rs.Label[j]);
                        strs[str] = i.Data[j];
                    }
                    dictionaries.Add(strs);
                }
            }
            return dictionaries;
        }

        public static List<dynamic> fetch_object(Result rs)
        {
            List<object> objs = new List<object>();
            foreach (Dictionary<string, object> strs in fetch_array(rs))
            {
                ExpandoObject expandoObjects = new ExpandoObject();
                ICollection<KeyValuePair<string, object>> keyValuePairs = expandoObjects;
                foreach (KeyValuePair<string, object> keyValuePair in strs)
                {
                    keyValuePairs.Add(keyValuePair);
                }
                objs.Add(expandoObjects);
            }
            return objs;
        }

        public static string[] list_fields(Result rs)
        {
            string[] array = rs.Label.Select<string, string>(new Func<string, string>(readableString)).ToArray<string>();
            return array;
        }

        public static int num_fields(Result rs)
        {
            return (int)rs.Label.Length;
        }

        public static int num_rows(Result rs)
        {
            return (int)rs.Root.Data.Length;
        }

        public static Result query(string query)
        {
            Result result = db.Execute(query, myChannel);
            errormsg = result.Error;
            return result;
        }

        private static string readableString(string src)
        {
            char[] charArray = src.ToLower().ToCharArray();
            charArray[0] = Convert.ToChar(char.ToUpper(charArray[0]));
            return new string(charArray);
        }

        public static void rollback(Channel mc)
        {
            mc.Rollback();
        }

        public static SharpHsql.Database select_db(string name)
        {
            db = new SharpHsql.Database(name);

            return db;
        }
    }
}