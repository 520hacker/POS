using System;

namespace Pos.Internals.Extensions
{
    /// <summary>
    /// Weeks, Years, Days helper class.
    /// </summary>
    public abstract class TimeSelector
    {
        /// <summary>
        /// Used by class to calculate time differences.
        /// </summary>
        private TimeSpan timeSpan;

        /// <summary>
        /// Gets a time in the past.
        /// </summary>
        public DateTime Ago 
        { 
            get 
            {
                return DateTime.Now - this.timeSpan; 
            }
        }

        /// <summary>
        /// Gets a time in the future.
        /// </summary>
        public DateTime FromNow 
        { 
            get 
            {
                return DateTime.Now + this.timeSpan; 
            }
        }

        /// <summary>
        /// Sets an internal reference value.
        /// </summary>
        internal int ReferenceValue
        {
            set
            {
                this.timeSpan = this.MyTimeSpan(value);
            }
        }

        /// <summary>
        /// Determines time in past from a date.
        /// </summary>
        /// <param name="dt">Date for which to relate the past date.</param>
        /// <returns>Date prior to this instance.</returns>
        public DateTime AgoSince(DateTime dt)
        {
            return dt - this.timeSpan;
        }

        /// <summary>
        /// Determines a time in the future.
        /// </summary>
        /// <param name="dt">Date for which to relate the future date.</param>
        /// <returns>A future date.</returns>
        public DateTime From(DateTime dt)
        {
            return dt + this.timeSpan;
        }

        /// <summary>
        /// Abstract time span helper method.
        /// </summary>
        /// <param name="refValue">Reference value.</param>
        /// <returns>A time span.</returns>
        protected abstract TimeSpan MyTimeSpan(int refValue);
    }
}