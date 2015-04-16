using System;
using System.Collections.Generic;
using Pos.Internals.ScriptEngine.ModuleSystem;
using SharpHsql;
using Std.Internals;

namespace Std
{
    [ScriptModule(AsType=false)]
    public class HSqlModule
    {
        private static SqlHelper helper = new SqlHelper();

        [ScriptFunction(Name="hsql_select_db")]
        public static void SelectDB(string name)
        {
            helper.select_db(name);
        }

        [ScriptFunction(Name="hsql_connect")]
        public static void Connect(string name, string pw)
        {
            helper.connect(name, pw);
        }

        [ScriptFunction(Name = "hsql_error")]
        public static string Error()
        {
            return helper.error();
        }

        [ScriptFunction(Name = "hsql_escape_string")]
        public static void EscapeString(string name)
        {
            helper.escape_string(name);
        }

        [ScriptFunction(Name = "hsql_fetch_array")]
        public static List<Dictionary<string, object>> FetchArray(Result rs)
        {
            return helper.fetch_array(rs);
        }

        [ScriptFunction(Name = "hsql_fetch_object")]
        public static List<dynamic> FetchObject(Result rs)
        {
            return helper.fetch_object(rs);
        }

        [ScriptFunction(Name = "hsql_list_fields")]
        public static string[] ListFields(Result rs)
        {
            return helper.list_fields(rs);
        }

        [ScriptFunction(Name = "hsql_num_fields")]
        public static int NumFields(Result rs)
        {
            return helper.num_fields(rs);
        }

        [ScriptFunction(Name = "hsql_num_rows")]
        public static int NumRows(Result rs)
        {
            return helper.num_rows(rs);
        }

        [ScriptFunction(Name = "hsql_num_fields")]
        public static Result Query(string rs)
        {
            return helper.query(rs);
        }

        [ScriptFunction(Name = "hsql_rollback")]
        public static void Rollback(Result rs)
        {
            helper.rollback();
        }
    }
}