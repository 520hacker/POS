namespace Lib.JSON.Serialization
{
    internal enum JsonContractType
    {
        None,
        Object,
        Array,
        Primitive,
        String,
        Dictionary,
        #if !(NET35 || NET20 || WINDOWS_PHONE)
        Dynamic,
        #endif
        #if !SILVERLIGHT && !PocketPC
        Serializable,
        #endif
        Linq
    }
}