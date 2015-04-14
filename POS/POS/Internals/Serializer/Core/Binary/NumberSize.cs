using System;

namespace Polenter.Serialization.Core.Binary
{
    /// <summary>
    ///   How many bytes occupies a number value
    /// </summary>
    public static class NumberSize
    {
        ///<summary>
        ///  is zero
        ///</summary>
        public const byte Zero = 0;

        ///<summary>
        ///  serializes as 1 byte
        ///</summary>
        public const byte B1 = 1;

        ///<summary>
        ///  serializes as 2 bytes
        ///</summary>
        public const byte B2 = 2;

        ///<summary>
        ///  serializes as 4 bytes
        ///</summary>
        public const byte B4 = 4;

        /// <summary>
        ///   Gives the least required byte amount to store the number
        /// </summary>
        /// <param name = "value"></param>
        /// <returns></returns>
        public static byte GetNumberSize(int value)
        {
            if (value == 0)
            {
                return Zero;
            }
            if (value > Int16.MaxValue || value < Int16.MinValue)
            {
                return B4;
            }
            if (value < byte.MinValue || value > byte.MaxValue)
            {
                return B2;
            }
            return B1;
        }
    }
}