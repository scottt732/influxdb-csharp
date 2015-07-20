using System;
using System.Diagnostics;

namespace InfluxDB.Timestamp.Source
{
    public static class DateTimeStopwatchTimestampSource
    {
        private static readonly long EpochTicks = 621355968000000000;

        private static readonly long StartDistance;
        private static readonly Stopwatch Stopwatch;

        static DateTimeStopwatchTimestampSource()
        {
            var timestamp = SystemTimePreciseTimestampSource.IsAvailable ? SystemTimePreciseTimestampSource.UtcNow : DateTime.UtcNow;
            StartDistance = timestamp.Ticks - EpochTicks;
            Stopwatch = Stopwatch.StartNew();
        }

        public static DateTime UtcNow
        {
            get
            {
                var elapsedTicks = (long)((Stopwatch.ElapsedTicks / (double)Stopwatch.Frequency) * TimeSpan.TicksPerSecond);
                return new DateTime(EpochTicks + StartDistance + elapsedTicks);
            }
        }
    }
}