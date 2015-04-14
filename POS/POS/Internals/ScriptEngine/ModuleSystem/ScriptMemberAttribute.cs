using System;

namespace Pos.Internals.ScriptEngine.ModuleSystem
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ScriptMemberAttribute : Attribute
    {
        public string Name { get; set; }
    }
}