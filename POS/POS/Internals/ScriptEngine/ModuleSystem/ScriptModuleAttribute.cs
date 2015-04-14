using System;
using System.Linq;

namespace Pos.Internals.ScriptEngine.ModuleSystem
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptModuleAttribute : Attribute
    {
        public string Name { get; set; }
        public bool AsType { get; set; }
    }
}