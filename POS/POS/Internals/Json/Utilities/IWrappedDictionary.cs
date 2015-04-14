using System.Collections;

namespace Lib.JSON.Utilities
{
    internal interface IWrappedDictionary : IDictionary
    {
        object UnderlyingDictionary { get; }
    }
}