using System;
using System.IO;
using System.Linq;
using System.Text;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = false)]
    public class FileModule
    {
        [ScriptFunction(Name = "fopen")]
        public FileStream FOpen(string filename, string access)
        {
            return new FileStream(filename, FileMode.OpenOrCreate, FAccess(access));
        }

        [ScriptFunction(Name = "fread")]
        public string FRead(FileStream s, int count)
        {
            byte[] buffer = new byte[count];
            s.Read(buffer, 0, count);

            return Encoding.UTF8.GetString(buffer);
        }

        [ScriptFunction(Name = "filesize")]
        public int FileSize(string filename)
        {
            return (int)new FileInfo(filename).Length;
        }

        [ScriptFunction(Name = "fclose")]
        public void FClose(FileStream fs)
        {
            fs.Close();
        }

        private static FileAccess FAccess(string src)
        {
            switch (src)
            {
                case "r":
                    return FileAccess.Read;
                case "w":
                    return FileAccess.Write;
                case "rw":
                    return FileAccess.ReadWrite;
                default:
                    return FileAccess.ReadWrite;
            }
        }
    }
}
