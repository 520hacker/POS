using System;
using System.Collections.Generic;
using Pos.Internals.ScriptEngine.ModuleSystem;
using SharpHsql;
using Std.Internals;

namespace Std
{
    [ScriptModule(AsType=false)]
    public class MySqlModule
    {
        private SqlHelper helper = new SqlHelper();

        [ScriptFunction(Name="hsql_select_db")]
        public void SelectDB(string name) {
            helper.select_db(name);
        }

        [ScriptFunction(Name="hsql_connect")]
        public void Connect(string name, string pw) {
            helper.connect(name, pw);
        }

        [ScriptFunction(Name = "hsql_error")]
        public string Error()
        {
            return helper.error();
        }

        [ScriptFunction(Name = "hsql_escape_string")]
        public void EscapeString(string name)
        {
            helper.escape_string(name);
        }

        [ScriptFunction(Name = "hsql_fetch_array")]
        public List<Dictionary<string, object>> FetchArray(Result rs)
        {
            return helper.fetch_array(rs);
        }

        [ScriptFunction(Name = "hsql_fetch_object")]
        public List<dynamic> FetchObject(Result rs)
        {
            return helper.fetch_object(rs);
        }

        [ScriptFunction(Name = "hsql_list_fields")]
        public string[] ListFields(Result rs)
        {
            return helper.list_fields(rs);
        }

        [ScriptFunction(Name = "hsql_num_fields")]
        public int NumFields(Result rs)
        {
            return helper.num_fields(rs);
        }

        [ScriptFunction(Name = "hsql_num_rows")]
        public int NumRows(Result rs)
        {
            return helper.num_rows(rs);
        }

        [ScriptFunction(Name = "hsql_num_fields")]
        public Result Query(string rs)
        {
            return helper.query(rs);
        }

        [ScriptFunction(Name = "hsql_rollback")]
        public void Rollback(Result rs)
        {
            helper.rollback();
        }
    }
}