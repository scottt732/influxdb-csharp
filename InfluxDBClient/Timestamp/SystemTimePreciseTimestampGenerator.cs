using System;
using System.Diagnostics.CodeAnalysis;
using InfluxDB.Enums;
using InfluxDB.Timestamp.Source;

namespace InfluxDB.Timestamp
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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