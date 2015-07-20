using System.Diagnostics.CodeAnalysis;

namespace InfluxDB.Enums
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum Duration
    {
        Microsecond,
        Millisecond,
        Second,
        Minute,
        Hour,
        Day,
        Week
    }
}