using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InfluxDB.Extensions
{
    internal static class SeriesExtensions
    {
        public static IReadOnlyDictionary<string, object> ToSimpleDictionary(this Response.Series series)
        {
            if (series.Values.Length == 0)
            {
                throw new InfluxException("Series did not contain any Value Sets");
            }
            else if (series.Values.Length > 1)
            {
                throw new InfluxException("Series contained an unexpected number of Value Sets");
            }

            var columnNames = series.Columns;
            var values = series.Values[0];

            if (columnNames.Length != values.Length)
            {
                throw new InfluxException("Series contained a mismatched number of column names and values.  This likely indicates an InfluxDB bug.");
            }

            var result = new Dictionary<string, object>();
            for (var x = 0; x < columnNames.Length; x++)
            {
                result[columnNames[x]] = values[x];
            }

            return new ReadOnlyDictionary<string, object>(result);
        }

        public static IReadOnlyDictionary<string, List<object>> ToMultiValuedDictionary(this Response.Series series)
        {
            var columnNames = series.Columns;

            var result = new Dictionary<string, List<object>>();

            var firstValueSet = true;
            var columnCount = columnNames.Length;
            foreach (object[] valueSet in series.Values)
            {
                if (columnCount != valueSet.Length)
                {
                    throw new InfluxException("Series contained a mismatched number of column names and values.  This likely indicates an InfluxDB bug.");
                }

                for (var x = 0; x < columnNames.Length; x++)
                {
                    if (firstValueSet)
                    {
                        result[columnNames[x]] = new List<object>(new[] { valueSet[x] });
                    }
                    else
                    {
                        result[columnNames[x]].Add(valueSet[x]);
                    }
                }

                firstValueSet = false;
            }

            return new ReadOnlyDictionary<string, List<object>>(result);
        }
    }
}
