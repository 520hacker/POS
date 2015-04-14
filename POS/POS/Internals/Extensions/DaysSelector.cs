using System;

namespace Pos.Internals.Extensions
{
    /// <summary>
    /// Days helper class.
    /// </summary>
    public class DaysSelector : TimeSelector
    {
        /// <summary>
        /// Abstract time span helper method.
        /// </summary>
        /// <param name="refValue">Reference value.</param>
        /// <returns>A time span.</returns>
        protected override TimeSpan MyTimeSpan(int refValue) 
        { 
            return new TimeSpan(refValue, 0, 0, 0); 
        }
    }
}