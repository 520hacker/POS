using System;
using System.Linq;
using System.Reflection;
using Microsoft.ClearScript.Windows;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace POS.Internals.ScriptEngine.ModuleSystem
{
    public static class ModuleLoader
    {
        public delegate object StaticMethodFunc(params object[] args);

        public static void Load(this JScriptEngine se, Type t)
        {
            var ca = t.GetCustomAttribute<ScriptModuleAttribute>();

            if (ca != null)
            {
                object tmp;
                try
                {
                    tmp = Activator.CreateInstance(t);
                }
                catch { tmp = 12; }

                if (ca.AsType)
                {
                    se.AddHostType(ca.Name != null ? ca.Name : t.Name, t);
                }

                foreach (var me in t.GetMethods())
                {
                    if (me.IsStatic)
                    {
                        var meca = me.GetCustomAttribute<ScriptFunctionAttribute>();
                        if (meca != null)
                        {
                            se.AddHostObject(meca.Name != null ? meca.Name : me.Name, new StaticMethodFunc(args => me.Invoke(null, args)));
                        }
                    }
                }

                foreach (var me in t.GetProperties())
                {
                    var meca = me.GetCustomAttribute<ScriptMemberAttribute>();
                    if (meca != null)
                    {
                        se.AddHostObject(meca.Name != null ? meca.Name : me.Name, me.GetValue(tmp, null));
                    }
                }
            }
        }

        public static void Load(this JScriptEngine se, Assembly ass)
        {
            foreach (var t in ass.GetTypes())
            {
                if(!t.IsAbstract)
                    Load(se, t);
            }
        }
    }
}