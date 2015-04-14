namespace Polenter.Serialization.Core.Binary
{
    /// <summary>
    ///   These attributes are used during the binary serialization. They should be unique from Elements and SubElements.
    /// </summary>
    public class Attributes
    {
        ///<summary>
        ///</summary>
        public const byte DimensionCount = 101;

        ///<summary>
        ///</summary>
        public const byte ElementType = 102;

        ///<summary>
        ///</summary>
        public const byte Indexes = 103;

        ///<summary>
        ///</summary>
        public const byte KeyType = 104;

        ///<summary>
        ///</summary>
        public const byte Length = 105;

        ///<summary>
        ///</summary>
        public const byte LowerBound = 106;

        ///<summary>
        ///</summary>
        public const byte Name = 107;

        ///<summary>
        ///</summary>
        public const byte Type = 108;

        ///<summary>
        ///</summary>
        public const byte Value = 109;

        ///<summary>
        ///</summary>
        public const byte ValueType = 110;
    }
}