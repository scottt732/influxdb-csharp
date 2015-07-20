using System;
using InfluxDB.Enums;

namespace InfluxDB.Timestamp
{
    public class SystemTimePreciseTimestampGenerator : ITimestampGenerator
    {
        public TimePrecision Precision { get; private set; }

        public SystemTimePreciseTimestampGenerator(TimePrecision precision)
        {
            Precision = precision;
        }

        public DateTime? GetTimestamp()
        {
            return SystemTimePreciseTimestampSource.UtcNow;
        }
    }
}