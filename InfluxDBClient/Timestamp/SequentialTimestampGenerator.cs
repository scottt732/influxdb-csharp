using System;
using System.Diagnostics.CodeAnalysis;
using InfluxDB.Enums;

namespace InfluxDB.Timestamp
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class SequentialTimestampGenerator : ITimestampGenerator
    {
        private static readonly long EpochTicks = 621355968000000000;

        private long _tickCount;

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