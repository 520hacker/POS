namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   Every array is composed of dimensions. Singledimensional arrays have only one info,
    ///   multidimensional have more dimension infos.
    /// </summary>
    public sealed class DimensionInfo
    {
        /// <summary>
        ///   Start index for the array
        /// </summary>
        public int LowerBound { get; set; }

        /// <summary>
        ///   How many items are in this dimension
        /// </summary>
        public int Length { get; set; }
    }
}