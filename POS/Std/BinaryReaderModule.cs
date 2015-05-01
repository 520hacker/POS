using System;
using System.IO;
using System.Linq;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = true, Name = "BinaryReader")]
    public class BinaryReaderModule : BinaryReader
    {
        public BinaryReaderModule(System.IO.Stream input)
            : base(input)
        {
        }
    }
    [ScriptModule(AsType = true, Name = "BinaryWriter")]
    public class BinaryWriterModule : BinaryWriter
    {
        
    }
}