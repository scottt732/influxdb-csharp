using System;

namespace InfluxDB
{
    public class InfluxException : Exception
    {
        public int StatusCode { get; private set; }


        public InfluxException(string message) : base(message)
        {
        }

        public InfluxException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public InfluxException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InfluxException(string message, int statusCode, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}