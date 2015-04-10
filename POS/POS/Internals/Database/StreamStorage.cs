using System;
using System.Collections.Generic;
using System.IO;
using Pos.Internals.Extensions;

namespace POS.Internals.Database
{
    public class StreamStorage
    {
        public static MemoryStream[] Split(Stream strm)
        {
            var br = new BinaryReader(strm);
            var count = br.ReadInt32();

            var ret = new List<MemoryStream>();

            for (int i = 0; i < count; i++)
            {
                var c = br.ReadInt32();

                br.ReadBytes(c);
            }

            return ret.ToArray();
        }

        public static Stream Combine(Stream[] strms)
        {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);

            bw.Write(strms.Length);
            foreach (var s in strms)
            {
                bw.Write(s.Length);
                bw.Write(s.CopyToMemory().ToArray());
            }

            return ms;
        }
    }
}