using System;
using System.Text.RegularExpressions;

namespace InfluxDB.Extensions
{
    internal static class StringExtensions
    {
        private static readonly Regex UnquotedIdentifier = new Regex("[A-Za-z_][A-Za-z0-9_]*");

        public static string EscapeMeasurementName(this string str)
        {
            return str.Replace(" ", @"\ ");
        }

        public static string EscapeFieldKey(this string str)
        {
            return str.Replace(" ", "\\ ").Replace(",", "\\,");
        }

        public static string EscapeTagName(this string str)
        {
            return str.Replace(" ", "\\ ").Replace(",", "\\,");
        }

        public static string EscapeTagValue(this string str)
        {
            return str.Replace(@"\,", @"\\,").Replace(" ", @"\ ").Replace(",", @"\,");
        }

        public static string EscapeFieldValueString(this string str)
        {
            return str.Replace("\"", "\\\"");
        }

        public static string StripWhitespace(this string str)
        {
            return string.Join(" ", str.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)).Trim();
        }

        public static string FormatIdentifier(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("The identifier is invalid");
            }

            if (UnquotedIdentifier.IsMatch(str)) return str;

            var alreadyQuoted = str.StartsWith("\"") && str.Length > 1 && str.EndsWith("\"");
            var body = alreadyQuoted ? str.Substring(1, str.Length - 2) : str.Trim();

            return "\"" + body.Replace("\"", "\\\"") + "\"";
        }
    }
}