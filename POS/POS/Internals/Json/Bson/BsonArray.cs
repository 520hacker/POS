using System.Collections;
using System.Collections.Generic;

namespace Lib.JSON.Bson
{
    internal class BsonArray : BsonToken, IEnumerable<BsonToken>
    {
        private readonly List<BsonToken> _children = new List<BsonToken>();

        public void Add(BsonToken token)
        {
            this._children.Add(token);
            token.Parent = this;
        }

        public override BsonType Type
        {
            get
            {
                return BsonType.Array;
            }
        }

        public IEnumerator<BsonToken> GetEnumerator()
        {
            return this._children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}