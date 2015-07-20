using System;
using System.Collections.Generic;
using InfluxDB.Extensions;

namespace InfluxDB
{
    public class InfluxKey
    {
        public string MeasurementName { get; private set; }

        public IDictionary<string, object> Tags
        {
            get { return _tags; }
        }

        private readonly SortedDictionary<string, object> _tags;

        public InfluxKey(string measurementName)
        {
            MeasurementName = measurementName.Trim();
            _tags = new SortedDictionary<string, object>(StringComparer.Ordinal);
        }

        public InfluxKey(string measurementName, IDictionary<string, object> tags)
        {
            MeasurementName = measurementName.Trim();
            _tags = tags == null ? new SortedDictionary<string, object>(StringComparer.Ordinal) : tags.ToValidatedSortable();
        }

        public override string ToString()
        {
            if (_tags == null || _tags.Count == 0) return MeasurementName;            
            return MeasurementName.EscapeMeasurementName() + "," + _tags.FormatTags();
        }
    }
}