using System;
using System.Diagnostics.CodeAnalysis;
using InfluxDB.Enums;
using InfluxDB.Timestamp.Source;

namespace InfluxDB.Timestamp
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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