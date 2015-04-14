using System;

namespace Pos.Internals.Extensions
{
    /// <summary>
    /// Weeks helper class.
    /// </summary>
    public class WeekSelector : TimeSelector
    {
        /// <summary>
        /// Abstract time span helper method.
        /// </summary>
        /// <param name="refValue">Reference value.</param>
        /// <returns>A time span.</returns>
        protected override TimeSpan MyTimeSpan(int refValue) 
        { 
            return new TimeSpan(7 * refValue, 0, 0, 0); 
        }
    }
}