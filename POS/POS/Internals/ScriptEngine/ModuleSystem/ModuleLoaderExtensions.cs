using System;
using System.Linq;
using System.Reflection;
using Microsoft.ClearScript.Windows;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace POS.Internals.ScriptEngine.ModuleSystem
{
    public static class ModuleLoaderExtensions
    {
        public static void Load(this WindowsScriptEngine se, Assembly ass)
        {
            foreach (var t in ass.GetTypes())
            {
                var ca = t.GetCustomAttribute<ScriptModuleAttribute>();

                se.AddHostType(t.Name, t);

                if (ca != null)
                {
                    foreach (var me in t.GetMethods())
                    {
                        var meca = me.GetCustomAttribute<ScriptFunctionAttribute>();
                        if (meca != null)
                        {
                            se.AddHostObject(me.Name, Delegate.CreateDelegate(t, me));
                        }
                    }
                    foreach (var me in t.GetProperties())
                    {
                        var meca = me.GetCustomAttribute<ScriptMemberAttribute>();
                        if (meca != null)
                        {
                            var tmp = Activator.CreateInstance(t);

                            se.AddHostObject(me.Name, me.GetValue(tmp, null));
                        }
                    }
                }
            }
        }
    }
}