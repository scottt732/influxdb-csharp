using System;
using System.Text.RegularExpressions;

namespace InfluxDB
{
    public enum RetentionUnit
    {
        Hours,
        Days,
        Weeks,
        Infinite
    }

    public class Retention
    {
        public int Amount { get; set; }
        public RetentionUnit Unit { get; set; }

        private Retention(int amount, RetentionUnit unit)
        {
            if (amount < 1)
            {
                throw new ArgumentException("The Retention Amount must be >= 1");
            }

            Amount = amount;
            Unit = unit;
        }

        public static Retention Hours(int hours)
        {
            return new Retention(hours, RetentionUnit.Hours);
        }

        public static Retention Days(int days)
        {
            return new Retention(days, RetentionUnit.Days);
        }

        public static Retention Weeks(int weeks)
        {
            return new Retention(weeks, RetentionUnit.Weeks);
        }

        public static Retention Infinite()
        {
            return new Retention(int.MaxValue, RetentionUnit.Infinite);
        }

        public static bool TryParse(string retention, out Retention result)
        {
            try
            {
                result = Parse(retention);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }

        private static readonly Regex Parser = new Regex(@"^(<value>\d+)(?<unit>[hdw])");

        public static Retention Parse(string retention)
        {
            if (retention == null)
            {
                throw new ArgumentNullException("retention");
            }
            if (string.IsNullOrEmpty(retention))
            {
                throw new ArgumentException("Retention argument is required");
            }

            if (retention.Equals("INF", StringComparison.OrdinalIgnoreCase))
            {
                return Infinite();
            }

            var match = Parser.Match(retention);
            if (match.Success)
            {
                var value = int.Parse(match.Groups["value"].Value);
                var unit = match.Groups["unit"].Value;

                switch (unit)
                {
                    case "h":
                        return Hours(value);
                    case "d":
                        return Days(value);
                    case "w":
                        return Weeks(value);
                    default:
                        throw new InvalidOperationException("Unsupported unit: " + unit);
                }
            }
            else
            {
                throw new ArgumentException("Retention argument is invalid");
            }
        }

        public override string ToString()
        {
            if (Amount == int.MaxValue && Unit == RetentionUnit.Infinite)
            {
                return "INF";
            }

            switch (Unit)
            {
                case RetentionUnit.Hours:
                    return Amount + "h";
                case RetentionUnit.Days:
                    return Amount + "d";
                case RetentionUnit.Weeks:
                    return Amount + "w";
                default:
                    throw new InvalidOperationException("Unsupported Retention Unit: " + Unit);
            }
        }
    }
}
