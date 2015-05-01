using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Creek.Compression.Zip;
using Pos.Internals.ScriptEngine.ModuleSystem;

namespace Std
{
    [ScriptModule(AsType = true, Name="ZipFile")]
    public class ZipModule
    {
        ZipFile _zip;

        public ZipModule(Stream filename)
        {
            _zip = new ZipFile(filename);
        }

        public void Close()
        {
            _zip.Close();
        }

        public Stream ReadFile(string name)
        {
            return _zip.GetInputStream(_zip.GetEntry(name));
        }

        public IEnumerable<string> GetFileNames()
        {
            foreach (ZipEntry i in _zip)
            {
                if (!i.IsDirectory)
                {
                    yield return i.Name;
                }
            }
        }

        public void AddFile(string filename, string entryName)
        {
            _zip.BeginUpdate();
            _zip.Add(filename, entryName);
            _zip.CommitUpdate();
        }
    }
}