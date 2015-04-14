using System;

namespace Lib.JSON.Serialization
{
    internal interface IMetadataTypeAttribute
    {
        Type MetadataClassType { get; }
    }
}