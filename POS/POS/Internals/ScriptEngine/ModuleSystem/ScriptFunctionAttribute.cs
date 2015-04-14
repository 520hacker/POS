using System;

namespace Pos.Internals.ScriptEngine.ModuleSystem
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ScriptFunctionAttribute : Attribute
    {
        public string Name { get; set; }
    }
}