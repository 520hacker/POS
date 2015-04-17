using System;
using System.Linq;

namespace POS.Internals
{
    public class SizeFormatter
    {
        public static string Format(double len, int decimals)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            return String.Format("{0:0." + new string('#', decimals) + "} {1}", len, sizes[order]);
        }

        public static string ToString(byte[] raw, int decimals)
        {
            return Format(raw.Length, decimals);
        }

    }
}