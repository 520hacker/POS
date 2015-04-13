using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Polenter.Serialization;

namespace POS.Internals.FileSystem
{
    public class MemoryFileSystem : SharpFileSystem.IFileSystem
    {
        public abstract class IEntity
        {
            public string Name { get; set; }
        }
        public class Folder : IEntity
        {
            public List<IEntity> Items = new List<IEntity>();
        }
        public class File : IEntity
        {
            public byte[] Content { get; set; }
        }

        private List<IEntity> _file = new List<IEntity>();
        private Stream _strm;

        private MemoryFileSystem(ref Stream strm)
        {
            var s = new SharpSerializer();
            _file = (List<IEntity>)s.Deserialize(strm);
            _strm = strm;
        }

        public void Dispose()
        {
            var s = new SharpSerializer();
            s.Serialize(_file, _strm);
        }

        protected SharpFileSystem.FileSystemPath ToPath(ZipEntry entry)
        {
            return SharpFileSystem.FileSystemPath.Parse(entry.Name);
        }

        protected string ToEntryPath(SharpFileSystem.FileSystemPath path)
        {
            return path.ToString();
        }

        public ICollection<SharpFileSystem.FileSystemPath> GetEntities(SharpFileSystem.FileSystemPath path)
        {
            return GetZipEntries().Select(e => ToPath(e)).Where(p => p.ParentPath == path).ToList();
        }

        public bool Exists(SharpFileSystem.FileSystemPath path)
        {
            return _file.Select(c => c.Name == path.EntityName).Any();
        }

        public Stream CreateFile(SharpFileSystem.FileSystemPath path)
        {
            var entry = new MemoryZipEntry();
            ZipFile.Add(entry, ToEntryPath(path));
            return entry.GetSource();
        }

        public Stream OpenFile(SharpFileSystem.FileSystemPath path, FileAccess access)
        {
            if (access != FileAccess.Read)
                throw new NotSupportedException();
            return ZipFile.GetInputStream(ToEntry(path));
        }

        public void CreateDirectory(SharpFileSystem.FileSystemPath path)
        {
            ZipFile.AddDirectory(ToEntryPath(path));
        }

        public void Delete(SharpFileSystem.FileSystemPath path)
        {
            ZipFile.Delete(ToEntryPath(path));
        }
    }
}
