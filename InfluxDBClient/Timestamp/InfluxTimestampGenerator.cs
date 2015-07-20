using System;
using InfluxDB.Enums;

namespace InfluxDB.Timestamp
{
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