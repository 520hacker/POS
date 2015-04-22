using System;
using System.IO;
using System.Linq;
using System.Text;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = false)]
    public class XmlModule
    {
        [ScriptFunction(Name = "fopen")]
        public static FileStream FOpen(string filename, string access)
        {
            return new FileStream(filename, FileMode.OpenOrCreate, FAccess(access));
        }

        [ScriptFunction(Name = "fread")]
        public static string FRead(FileStream s, int count)
        {
            byte[] buffer = new byte[count];
            s.Read(buffer, 0, count);

            return Encoding.UTF8.GetString(buffer);
        }

        [ScriptFunction(Name = "filesize")]
        public static int FileSize(string filename)
        {
            return (int)new FileInfo(filename).Length;
        }

        [ScriptFunction(Name = "set_filesize")]
        public static void SetFileSize(string filename, int length)
        {
            var fs = new FileStream(filename, FileMode.OpenOrCreate);

            fs.SetLength(length);

            fs.Dispose();
            fs.Close();
        }

        [ScriptFunction(Name = "reserve_filespace")]
        public static void ReserveSpace(string filename, int length)
        {
            using (FileStream outFile = System.IO.File.Create(filename))
            {
                outFile.Seek(length - 1, SeekOrigin.Begin);
                outFile.WriteByte(0);
            }
        }

        [ScriptFunction(Name = "fclose")]
        public static void FClose(FileStream fs)
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
