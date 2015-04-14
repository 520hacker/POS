using System.Collections;

namespace Lib.JSON.Utilities
{
    internal interface IWrappedCollection : IList
    {
        object UnderlyingCollection { get; }
    }
}