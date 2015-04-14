using System.Collections;

namespace Lib.JSON.Utilities
{
    internal interface IWrappedList : IList
    {
        object UnderlyingList { get; }
    }
}