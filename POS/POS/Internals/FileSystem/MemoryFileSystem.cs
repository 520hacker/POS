using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Polenter.Serialization;

namespace Pos.Internals.FileSystem
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

        private readonly List<IEntity> _file = new List<IEntity>();
        private readonly Stream _strm;

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

        protected string ToEntryPath(SharpFileSystem.FileSystemPath path)
        {
            return path.ToString();
        }

        public ICollection<SharpFileSystem.FileSystemPath> GetEntities(SharpFileSystem.FileSystemPath path)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public bool Exists(SharpFileSystem.FileSystemPath path)
        {
            return _file.Select(c => c.Name == path.EntityName).Any();
        }

        public Stream CreateFile(SharpFileSystem.FileSystemPath path)
        {
            var f = new File();
            var ms = new MemoryStream();

            f.Name = path.EntityName;
            f.Content = ms.ToArray();

            _file.Add(f);

            return ms;
        }

        public Stream OpenFile(SharpFileSystem.FileSystemPath path, FileAccess access)
        {
            foreach (var item in _file)
            {
                if (item.Name == path.EntityName)
                {
                    if (item is File)
                    {
                        return new MemoryStream(((File)item).Content);
                    }
                }
            }

            return null;
        }

        public void CreateDirectory(SharpFileSystem.FileSystemPath path)
        {
            var d = new Folder();
            d.Name = ToEntryPath(path);

            _file.Add(d);
        }

        public void Delete(SharpFileSystem.FileSystemPath path)
        {
            for (int i = 0; i < _file.Count; i++)
            {
                if (_file[i].Name == ToEntryPath(path))
                {
                    _file.RemoveAt(i);
                }
            }
        }
    }
}