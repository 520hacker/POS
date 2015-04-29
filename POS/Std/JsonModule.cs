using System;
using System.IO;
using System.Linq;
using POS.Internals.I18N;
using Polenter.Serialization;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = true, Name="JSON")]
    public class JsonModule
    {
        public static string stringify(object obj)
        {
            return Lib.JSON.JsonConvert.SerializeObject(obj);
        }

        public static object parse(string json)
        {
            return Lib.JSON.JsonConvert.DeserializeObject(json);
        }
    }
}
