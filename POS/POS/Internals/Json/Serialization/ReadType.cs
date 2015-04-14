namespace Lib.JSON.Serialization
{
    internal enum ReadType
    {
        Read,
        ReadAsInt32,
        ReadAsDecimal,
        ReadAsBytes,
        #if !NET20
        ReadAsDateTimeOffset
        #endif
    }
}