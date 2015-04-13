using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using SharpHsql;

namespace POS.Internals
{
    public class SqlHelper
    {
        private SharpHsql.Database db;

        private string errormsg;

        public SqlHelper()
        {

        }

        public void auto_commit(Channel mc, bool b)
        {
            mc.SetAutoCommit(b);
        }

        public void close(Channel myChannel)
        {
            this.db.Execute("shutdown", myChannel);
            myChannel.Disconnect();
        }

        public void commit(Channel myChannel)
        {
            myChannel.Commit();
        }

        public Channel connect(string username, string password)
        {
            return this.db.Connect(username, password);
        }

        public string error()
        {
            return this.errormsg;
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
                        string str = this.readableString(rs.Label[j]);
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
            foreach (Dictionary<string, object> strs in this.fetch_array(rs))
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
            string[] array = rs.Label.Select<string, string>(new Func<string, string>(this.readableString)).ToArray<string>();
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

        public Result query(Channel myChannel, string query)
        {
            Result result = this.db.Execute(query, myChannel);
            this.errormsg = result.Error;
            return result;
        }

        private string readableString(string src)
        {
            char[] charArray = src.ToLower().ToCharArray();
            charArray[0] = Convert.ToChar(charArray[0].ToString().ToUpper());
            return new string(charArray);
        }

        public void rollback(Channel mc)
        {
            mc.Rollback();
        }

        public void select_db(string name)
        {
            this.db = new SharpHsql.Database(name);
        }
    }
}