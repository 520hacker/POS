using System;
using System.Linq;
using System.Windows.Forms;
using POS.Internals;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = false)]
    public class UtilitiesModule
    {
        [ScriptFunction(Name = "base64_encode")]
        public static string Base64Encode(string raw)
        {
            return Pos.Internals.Extensions.StringExtensions.Base64Encode(raw);
        }

        [ScriptFunction(Name = "size_format")]
        public static string SizeFormat(int target, int deci)
        {
            return SizeFormatter.Format(target, deci);
        }

        [ScriptFunction(Name = "alert")]
        public static void Alert(string content)
        {
            MessageBox.Show(content);
        }

        [ScriptFunction(Name = "base64_decode")]
        public static string Base64Decode(string raw)
        {
            return Pos.Internals.Extensions.StringExtensions.Base64Decode(raw);
        }

        [ScriptFunction(Name = "iif")]
        public static object IIF(bool condition, object t, object f)
        {
            return Pos.Internals.Extensions.Extensions.IIf(condition, t, f);
        }
    }
}