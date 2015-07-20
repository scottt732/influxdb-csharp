using System;
using System.Diagnostics.CodeAnalysis;
using InfluxDB.Enums;

namespace InfluxDB.Timestamp
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class InfluxTimestampGenerator : ITimestampGenerator
    {
        public TimePrecision Precision { get; private set; }

        public InfluxTimestampGenerator(TimePrecision precision)
        {
            Precision = precision;
        }

        public DateTime? GetTimestamp()
        {
            return null;
        }
    }
}