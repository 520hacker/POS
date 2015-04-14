namespace Pos.Internals.Extensions
{
    /// <summary>
    /// Available system image list sizes
    /// </summary>
    public enum SysImageListSize : int
    {
        /// <summary>
        /// System Large Icon Size (typically 32x32)
        /// </summary>
        largeIcons = 0x0,

        /// <summary>
        /// System Small Icon Size (typically 16x16)
        /// </summary>
        smallIcons = 0x1,

        /// <summary>
        /// System Extra Large Icon Size (typically 48x48).
        /// Only available under XP; under other OS the
        /// Large Icon ImageList is returned.
        /// </summary>
        extraLargeIcons = 0x2
    }
}