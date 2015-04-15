using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.ClearScript.Windows;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace POS.Internals.ScriptEngine.ModuleSystem
{
    public static class ModuleLoader
    {
        public static void Load(this WindowsScriptEngine se, Type t)
        {
            var ca = t.GetCustomAttribute<ScriptModuleAttribute>();

            if (ca.AsType)
            {
                se.AddHostType(ca.Name != null ? ca.Name : t.Name, t);
            }

            if (ca != null)
            {
                foreach (var me in t.GetMethods())
                {
                    var meca = me.GetCustomAttribute<ScriptFunctionAttribute>();
                    if (meca != null)
                    {
                        //se.AddHostObject(meca.Name != null ? meca.Name : me.Name, me.(t));
                    }
                }
            }
            foreach (var me in t.GetProperties())
            {
                var meca = me.GetCustomAttribute<ScriptMemberAttribute>();
                if (meca != null)
                {
                    var tmp = Activator.CreateInstance(t);

                    se.AddHostObject(meca.Name != null ? meca.Name : me.Name, me.GetValue(tmp, null));
                }
            }
        }

        public static void Load(this WindowsScriptEngine se, Assembly ass)
        {
            foreach (var t in ass.GetTypes())
            {
                Load(se, t);
            }
        }
    }
}