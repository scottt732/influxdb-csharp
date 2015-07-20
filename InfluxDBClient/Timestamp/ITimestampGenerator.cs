using System;
using InfluxDB.Enums;

namespace InfluxDB.Timestamp
{
    public interface ITimestampGenerator
    {
        TimePrecision Precision { get; }

        DateTime? GetTimestamp();
    }
}