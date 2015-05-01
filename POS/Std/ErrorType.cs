using System;
using System.Linq;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = true, Name="Error")]
    public class ErrorType
    {
        public ErrorType(string msg)
        {
            ErrorMessage = msg;
        }

        public string ErrorMessage { get; set; }
    }
}