namespace Lib.JSON.Bson
{
    internal class BsonValue : BsonToken
    {
        private readonly BsonType _type;

        public BsonValue(object value, BsonType type)
        {
            this.Value = value;
            this._type = type;
        }

        public object Value { get; private set; }

        public override BsonType Type
        {
            get
            {
                return this._type;
            }
        }
    }
}