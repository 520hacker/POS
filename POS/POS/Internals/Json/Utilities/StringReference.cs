using System;
using System.Linq;

namespace Lib.JSON.Utilities
{
    internal struct StringReference
    {
        public StringReference(char[] chars, int startIndex, int length) : this()
        {
            this.Chars = chars;
            this.StartIndex = startIndex;
            this.Length = length;
        }

        public char[] Chars { get; private set; }

        public int StartIndex { get; private set; }

        public int Length { get; private set; }

        public override string ToString()
        {
            return new string(this.Chars, this.StartIndex, this.Length);
        }
    }
}