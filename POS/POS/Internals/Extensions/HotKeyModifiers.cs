using System;

namespace Pos.Internals.Extensions
{
    /// <summary>
    /// HotKeyModifiers énumération
    /// </summary>
    [Flags]
    public enum HotKeyModifiers : int
    {
        Alt = 0x1,
        Control = 0x2,
        Shift = 0x4,
        Windows = 0x8
    }
}