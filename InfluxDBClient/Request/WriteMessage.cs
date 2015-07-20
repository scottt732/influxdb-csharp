using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using InfluxDB.Enums;
using InfluxDB.Extensions;

namespace InfluxDB.Request
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class WriteMessage
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public InfluxKey Key { get; private set; }
        public IDictionary<string, object> Fields { get { return _fields; } }

        private readonly SortedDictionary<string, object> _fields;
         
        public WriteMessage(string key)
        {
            Key = new InfluxKey(key);
            _fields = new SortedDictionary<string, object>(StringComparer.Ordinal);
        }

        public WriteMessage(string key, IDictionary<string, object> fields)
        {
            Key = new InfluxKey(key);
            _fields = fields == null ? new SortedDictionary<string, object>(StringComparer.Ordinal) : fields.ToValidatedSortable();
        }

        public WriteMessage(InfluxKey key)
        {
            Key = key;
            _fields = new SortedDictionary<string, object>(StringComparer.Ordinal);
        }

        public WriteMessage(InfluxKey key, IDictionary<string, object> fields)
        {
            Key = key;
            _fields = fields == null ? new SortedDictionary<string, object>(StringComparer.Ordinal) : fields.ToValidatedSortable();
        }

        public string ToString(DateTime? timestamp, TimePrecision? precision, double comparisonTolerance = 0.00001)
        {
            return Key + " " + Fields.FormatFields(comparisonTolerance) + (timestamp.HasValue ? " " + ToUnixTime(timestamp.Value, (precision ?? TimePrecision.Nanosecond)) : string.Empty);
        }

        public static long ToUnixTime(DateTime date, TimePrecision precision)
        {
            switch (precision)
            {
                case TimePrecision.Nanosecond:
                    return Convert.ToInt64((date.ToUniversalTime() - Epoch).TotalNanoseconds());
                case TimePrecision.Microsecond:
                    return Convert.ToInt64((date.ToUniversalTime() - Epoch).TotalMicroseconds());
                case TimePrecision.Millisecond:
                    return Convert.ToInt64((date.ToUniversalTime() - Epoch).TotalMilliseconds);
                case TimePrecision.Second:
                    return Convert.ToInt64((date.ToUniversalTime() - Epoch).TotalSeconds);
                case TimePrecision.Minute:
                    return Convert.ToInt64((date.ToUniversalTime() - Epoch).TotalMinutes);
                case TimePrecision.Hour:
                    return Convert.ToInt64((date.ToUniversalTime() - Epoch).TotalHours);
                default:
                    throw new ArgumentException("Unsupported precision: " + precision, "precision");
            }
        }
    }
}