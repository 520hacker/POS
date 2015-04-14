using System;
using System.Linq;

namespace Lib.JSON
{
    /// <summary>
    /// Instructs the <see cref="JsonSerializer"/> to use the specified constructor when deserializing that object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
    public sealed class JsonConstructorAttribute : Attribute
    {
    }
}