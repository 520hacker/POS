using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using SharpHsql;

namespace Std.Internals
{
    public class SqlHelper
    {
        private SharpHsql.Database db;
        private Channel myChannel;

        private string errormsg;

        public void auto_commit(bool b)
        {
            myChannel.SetAutoCommit(b);
        }

        public void close()
        {
            db.Execute("shutdown", myChannel);
            myChannel.Disconnect();
        }

        public void commit()
        {
            myChannel.Commit();
        }

        public void connect(string username, string password)
        {
            myChannel = db.Connect(username, password);
        }

        public string error()
        {
            return errormsg;
        }

        public string escape_string(string s)
        {
            return Regex.Escape(s);
        }

        public List<Dictionary<string, object>> fetch_array(Result rs)
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

        public List<dynamic> fetch_object(Result rs)
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

        public string[] list_fields(Result rs)
        {
            string[] array = rs.Label.Select<string, string>(new Func<string, string>(readableString)).ToArray<string>();
            return array;
        }

        public int num_fields(Result rs)
        {
            return (int)rs.Label.Length;
        }

        public int num_rows(Result rs)
        {
            return (int)rs.Root.Data.Length;
        }

        public Result query(string query)
        {
            Result result = db.Execute(query, myChannel);
            errormsg = result.Error;
            return result;
        }

        private string readableString(string src)
        {
            char[] charArray = src.ToLower().ToCharArray();
            charArray[0] = Convert.ToChar(char.ToUpper(charArray[0]));
            return new string(charArray);
        }

        public void rollback()
        {
            myChannel.Rollback();
        }

        public SharpHsql.Database select_db(string name)
        {
            db = new SharpHsql.Database(name);

            return db;
        }
    }
}