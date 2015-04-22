using System;
using System.IO;
using System.Linq;
using POS.Internals.I18N;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = false)]
    public class GettextModule
    {
        private static Catalog _catalog = new Catalog();

        [ScriptFunction(Name = "gettext_load")]
        public static Catalog Load(string filename)
        {
            _catalog.Load(filename);

            return _catalog;
        }

        [ScriptFunction(Name = "gettext_")]
        public static string GetString(string id)
        {
            return _catalog.GetString(id);
        }
    }
}
