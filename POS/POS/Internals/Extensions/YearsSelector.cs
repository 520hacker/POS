using System;

namespace Pos.Internals.Extensions
{
    /// <summary>
    /// Years helper class.
    /// </summary>
    public class YearsSelector : TimeSelector
    {
        /// <summary>
        /// Abstract time span helper method.
        /// </summary>
        /// <param name="refValue">Reference value.</param>
        /// <returns>A time span.</returns>
        protected override TimeSpan MyTimeSpan(int refValue)
        {
            return new TimeSpan(365 * refValue, 0, 0, 0);
        }
    }
}