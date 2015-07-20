using System;
using InfluxDB.Enums;

namespace InfluxDB.Timestamp
{
    public class DateTimeStopwatchTimestampGenerator : ITimestampGenerator
    {
        public TimePrecision Precision { get; private set; }

        public DateTimeStopwatchTimestampGenerator(TimePrecision precision)
        {
            Precision = precision;
        }

        public DateTime? GetTimestamp()
        {
            return DateTimeStopwatchTimestampSource.UtcNow;
        }
    }
}