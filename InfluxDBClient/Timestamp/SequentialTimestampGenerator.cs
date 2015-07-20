using System;
using InfluxDB.Enums;

namespace InfluxDB.Timestamp
{
    public class SequentialTimestampGenerator : ITimestampGenerator
    {
        private static readonly long EpochTicks = 621355968000000000;

        private long _tickCount = 0;

        public TimePrecision Precision { get; private set; }

        public SequentialTimestampGenerator(TimePrecision precision)
        {
            Precision = precision;
        }

        public DateTime? GetTimestamp()
        {
            return new DateTime(EpochTicks + _tickCount++);
        }
    }
}