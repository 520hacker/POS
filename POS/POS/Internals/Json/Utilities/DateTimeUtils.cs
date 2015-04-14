using System;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace Lib.JSON.Utilities
{
    internal static class DateTimeUtils
    {
        public static string GetUtcOffsetText(this DateTime d)
        {
            TimeSpan utcOffset = d.GetUtcOffset();

            return string.Format("{0}:{1}", utcOffset.Hours.ToString("+00;-00", CultureInfo.InvariantCulture), utcOffset.Minutes.ToString("00;00", CultureInfo.InvariantCulture));
        }

        public static TimeSpan GetUtcOffset(this DateTime d)
        {
            #if PocketPC || NET20
      return TimeZone.CurrentTimeZone.GetUtcOffset(d);
            #else
            return TimeZoneInfo.Local.GetUtcOffset(d);
            #endif
        }

        public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
        {
            switch (kind)
            {
                case DateTimeKind.Local:
                    return XmlDateTimeSerializationMode.Local;
                case DateTimeKind.Unspecified:
                    return XmlDateTimeSerializationMode.Unspecified;
                case DateTimeKind.Utc:
                    return XmlDateTimeSerializationMode.Utc;
                default:
                    throw MiscellaneousUtils.CreateArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value.");
            }
        }
    }
}
