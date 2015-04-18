using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.ClearScript;
using POS.Internals.ScriptEngine;
using POS.Internals.ScriptEngine.ModuleSystem;

namespace POS
{
    public static class PluginLoader
    {
        private static JScriptEngine _engine = new JScriptEngine();

        public static List<dynamic> Plugins { get; set; }

        public static void Eval(string src)
        {
            _engine.Evaluate(src);
        }

        public static object[] Call(string func, params object[] p)
        {
            var ret = new List<object>();
      
            var host = new HostFunctions();
            ((IScriptableObject)host).OnExposedToScriptCode(_engine);
            var del = (Delegate)host.func<object>(p.Length, _engine.Script[func]);

            ret.Add(del.DynamicInvoke(p));
            
            return ret.ToArray();
        }

        public static void AddObject(string name, object obj)
        {
            _engine.AddHostObject(name, obj);
        }

        public static void AddType(string name, Type type)
        {
            _engine.AddHostType(name, type);
        }

        public static dynamic[] Load(string startupPath)
        {
            var ret = new List<dynamic>();

            _engine.AddHostObject("ns", new Action<string, string>((ns, n) =>
            {
                _engine.Execute(n + "=" + ns + ";");
            }));
            _engine.AddHostObject("host", new ExtendedHostFunctions());
            _engine.AddHostObject("clr", new ExtendedHostFunctions().lib("System", "System.Core", "System.Windows.Forms", typeof(Telerik.WinControls.UI.RadTextBoxControl).Assembly.FullName));


            foreach (var p in Directory.GetFiles(startupPath, "*.js"))
            {
                ModuleLoader.Load(_engine, Assembly.LoadFile(Application.StartupPath + "\\Std.dll"));

                ret.Add(_engine.Evaluate(File.ReadAllText(p)));
            }

            Plugins = ret;

            return ret.ToArray();
        }
    }
}