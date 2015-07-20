using System;
using InfluxDB.Enums;

namespace InfluxDB.Timestamp
{
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