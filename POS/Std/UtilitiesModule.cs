using System;
using System.Linq;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = false)]
    public class UtilitiesModule
    {
        [ScriptFunction(Name = "base64_encode")]
        public string Base64Encode(string raw)
        {
            return Pos.Internals.Extensions.StringExtensions.Base64Encode(raw);
        }

        [ScriptFunction(Name = "base64_decode")]
        public string Base64Decode(string raw)
        {
            return Pos.Internals.Extensions.StringExtensions.Base64Decode(raw);
        }

        [ScriptFunction(Name = "iif")]
        public object IIF(bool condition, object t, object f)
        {
            return Pos.Internals.Extensions.Extensions.IIf(condition, t, f);
        }
    }
}