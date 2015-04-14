using System.Collections;
using System.Collections.Generic;

namespace Lib.JSON.Bson
{
    internal abstract class BsonToken
    {
        public abstract BsonType Type { get; }

        public BsonToken Parent { get; set; }

        public int CalculatedSize { get; set; }
    }
}