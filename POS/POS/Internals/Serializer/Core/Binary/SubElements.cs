namespace Polenter.Serialization.Core.Binary
{
    /// <summary>
    ///   These elements are used during the binary serialization. They should be unique from Elements and Attributes.
    /// </summary>
    public static class SubElements
    {
        ///<summary>
        ///</summary>
        public const byte Dimension = 51;

        ///<summary>
        ///</summary>
        public const byte Dimensions = 52;

        ///<summary>
        ///</summary>
        public const byte Item = 53;

        ///<summary>
        ///</summary>
        public const byte Items = 54;

        ///<summary>
        ///</summary>
        public const byte Properties = 55;

        ///<summary>
        ///</summary>
        public const byte Unknown = 254;

        ///<summary>
        ///</summary>
        public const byte Eof = 255;
    }
}