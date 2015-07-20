using System;

namespace InfluxDB.Extensions
{
    public static class TimeSpanExtensions
    {
        public static long TotalNanoseconds(this TimeSpan self)
        {            
            return self.Ticks / 10;
        }

        public static long TotalMicroseconds(this TimeSpan self)
        {
            return self.Ticks * 100;
        }
    }
}
