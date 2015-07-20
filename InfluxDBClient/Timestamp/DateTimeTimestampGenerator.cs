using System;
using System.Diagnostics.CodeAnalysis;
using InfluxDB.Enums;

namespace InfluxDB.Timestamp
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class DateTimeTimestampGenerator : ITimestampGenerator
    {
        public TimePrecision Precision { get; private set; }

        public DateTimeTimestampGenerator(TimePrecision precision)
        {
            Precision = precision;
        }

        public DateTime? GetTimestamp()
        {
            return DateTime.UtcNow;
        }
    }
}