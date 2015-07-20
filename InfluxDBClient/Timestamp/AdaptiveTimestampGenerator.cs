using System;
using InfluxDB.Enums;
using InfluxDB.Timestamp.Source;

namespace InfluxDB.Timestamp
{
    public class AdaptiveTimestampGenerator : ITimestampGenerator
    {
        private readonly Func<DateTime> _generator;

        public TimePrecision Precision { get; private set; }

        private DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }

        private DateTime GetSystemTimePrecise()
        {
            return SystemTimePreciseTimestampSource.UtcNow;
        }

        private DateTime GetDateTimeStopwatch()
        {
            return DateTimeStopwatchTimestampSource.UtcNow;
        }

        public AdaptiveTimestampGenerator(TimePrecision precision)
        {
            Precision = precision;
            switch (precision)
            {
                case TimePrecision.Nanosecond:
                case TimePrecision.Microsecond:
                    _generator = SystemTimePreciseTimestampSource.IsAvailable
                        ? (Func<DateTime>) GetSystemTimePrecise
                        : GetDateTimeStopwatch;
                    break;
                default:
                    _generator = GetDateTime;
                    break;
            }
        }

        public DateTime? GetTimestamp()
        {
            return _generator();
        }
    }
}