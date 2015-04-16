using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.ClearScript;
using Microsoft.ClearScript.Windows;
using POS.Internals.ScriptEngine;
using POS.Internals.ScriptEngine.ModuleSystem;

namespace POS
{
    public static class PluginLoader
    {
        private static List<ScriptEngine> _engines = new List<ScriptEngine>();

        public static List<dynamic> Plugins { get; set; }

        public static object[] Call(string func, params object[] p)
        {
            var ret = new List<object>();

            foreach (var e in _engines)
            {
                var host = new HostFunctions();
                ((IScriptableObject)host).OnExposedToScriptCode(e);
                var del = (Delegate)host.func<object>(p.Length, e.Script[func]);

                ret.Add(del.DynamicInvoke(p));
            }

            return ret.ToArray();
        }

        public static dynamic[] Load(string startupPath)
        {
            var ret = new List<dynamic>();

            foreach (var p in Directory.GetFiles(startupPath))
            {
                WindowsScriptEngine se = null;

                if (p.EndsWith(".js"))
                {
                    se = new JScriptEngine();
                }
                else if (p.EndsWith(".vb"))
                {
                    se = new VBScriptEngine();
                }

                ModuleLoader.Load(se, Assembly.LoadFile(Application.StartupPath + "\\Std.dll"));

                ret.Add(se.Evaluate(File.ReadAllText(p)));

                _engines.Add(se);
            }

            Plugins = ret;

            return ret.ToArray();
        }
    }
}