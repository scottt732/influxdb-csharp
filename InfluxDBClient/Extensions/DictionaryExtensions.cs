using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace InfluxDB.Extensions
{
    internal static class DictionaryExtensions
    {
        private static bool IsValidType(object value)
        {
            return value is char || value is string || value is float || value is double || value is decimal || value is bool || value is int || value is uint || value is long || value is ulong || value is short || value is ushort || value is byte || value is sbyte;
        }

        public static SortedDictionary<string, object> ToValidatedSortable(this IDictionary<string, object> dictionary)
        {
            List<string> badKeys = null;

            var result = new SortedDictionary<string, object>(StringComparer.Ordinal);

            foreach (var tag in dictionary)
            {
                if (IsValidType(tag.Value))
                {
                    result[tag.Key] = tag.Value;
                }
                else
                {
                    if (badKeys == null)
                    {
                        badKeys = new List<string>(new[] {tag.Key});
                    }
                    else
                    {
                        badKeys.Add(tag.Key);
                    }
                }
            }

            if (badKeys != null)
            {
                throw new ArgumentException("Tags can only contain primitive value types.  Convert the following key values before adding: " + string.Join(", ", badKeys), "dictionary");
            }

            return result;
        }

        public static string FormatTags(this IDictionary<string, object> tags)
        {
            if (tags == null || tags.Count == 0) return "";
            var sb = new StringBuilder();
            foreach (var tag in tags)
            {
                var escapedTagName = tag.Key.EscapeTagName();
                var escapedTagValue = tag.Value.ToString().EscapeTagValue();
                sb.Append(escapedTagName + "=" + escapedTagValue + ",");
            }
            sb.Length -= 1;
            return sb.ToString();
        }

        public static string FormatFields(this IDictionary<string, object> fields, double comparisonTolerance = 0.00001)
        {
            List<string> badKeys = null;

            if (fields == null || fields.Count == 0) return "";
            var sb = new StringBuilder();
            foreach (var field in fields)
            {
                var escapedFieldName = field.Key.EscapeFieldKey();

                string escapedFieldValue = null;
                if (field.Value is char)
                {
                    escapedFieldValue = "\"" + field.Value + "\"";
                }
                if (field.Value is string)
                {
                    escapedFieldValue = "\"" + field.Value.ToString().EscapeFieldValueString() + "\"";
                }
                else if (field.Value is float || field.Value is double)
                {
                    escapedFieldValue = Math.Abs(((double) field.Value) - Math.Floor((double) field.Value)) < comparisonTolerance 
                        ? ((double) field.Value).ToString("F1")
                        : ((double)field.Value).ToString(CultureInfo.InvariantCulture);
                }
                else if (field.Value is decimal)
                {
                    escapedFieldValue = Math.Abs(((decimal)field.Value) - Math.Floor((decimal)field.Value)) < (decimal) comparisonTolerance
                        ? ((decimal)field.Value).ToString("F2")
                        : ((decimal)field.Value).ToString(CultureInfo.InvariantCulture);
                }
                if (field.Value is bool)
                {
                    escapedFieldValue = ((bool) field.Value) ? "true" : "false";
                }
                else if (field.Value is int || field.Value is uint || field.Value is long || field.Value is ulong || field.Value is short || field.Value is ushort || field.Value is byte || field.Value is sbyte)
                {
                    escapedFieldValue = field.Value.ToString();
                }
                else
                {
                    if (badKeys == null)
                    {
                        badKeys = new List<string>(new[] {field.Key});
                    }
                    else
                    {
                        badKeys.Add(field.Key);
                    }
                }

                if (escapedFieldValue != null)
                {
                    sb.Append(escapedFieldName + "=" + escapedFieldValue + ",");
                }
            }

            if (badKeys != null)
            {
                throw new ArgumentException("Tags can only contain primitive value types.  Convert the following key values before adding: " + string.Join(", ", badKeys), "fields");
            }

            sb.Length -= 1;
            return sb.ToString();
        }
    }
}