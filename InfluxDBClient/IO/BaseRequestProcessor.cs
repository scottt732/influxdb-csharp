using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Enums;
using InfluxDB.Extensions;
using InfluxDB.Request;
using InfluxDB.Response;

namespace InfluxDB.IO
{
    public abstract class BaseRequestProcessor : IRequestProcessor
    {
        public IRequestProcessorSettings Settings { get; private set; }

        public Task<ResultSet> SendQuery(string query, CancellationToken cancellationToken = new CancellationToken())
        {
            return SendQuery(null, query, TimePrecision.Microsecond, cancellationToken);
        }

        public Task<ResultSet> SendQuery(string database, string query, CancellationToken cancellationToken = new CancellationToken())
        {
            return SendQuery(database, query, TimePrecision.Microsecond, cancellationToken);
        }

        public Task<ResultSet> SendQuery(string query, TimePrecision resultPrecision = TimePrecision.Microsecond, CancellationToken cancellationToken = new CancellationToken())
        {
            return SendQuery(null, query, resultPrecision, cancellationToken);
        }

        public abstract Task<ResultSet> SendQuery(string database, string query, TimePrecision resultPrecision = TimePrecision.Microsecond, CancellationToken cancellationToken = new CancellationToken());
        public abstract Task SendCommand(string command);
        public abstract Task SendCommand(string database, string command);
        public abstract Task WriteMessage(string database, WriteMessage message, Consistency? consistency = null, string retentionPolicy = null, DateTime? timestamp = null, TimePrecision? precision = null);
        public abstract Task WriteMessages(string database, IEnumerable<WriteMessage> messages, Consistency? consistency = null, string retentionPolicy = null, DateTime? timestamp = null, TimePrecision? precision = null);

        protected BaseRequestProcessor(IRequestProcessorSettings settings)
        {
            Settings = settings;
        }

        protected string CreateUrl(string path, string database = null, string query = null, TimePrecision? timePrecision = null, Consistency? consistency = null, string retentionPolicy = null, TimePrecision? resultPrecision = null)
        {
            var queryString = new StringBuilder("?");

            if (!string.IsNullOrEmpty(query))
            {
                queryString.Append("q=" + Uri.EscapeDataString(query.StripWhitespace()) + "&");
            }

            if (!string.IsNullOrEmpty(database))
            {
                queryString.Append("db=" + Uri.EscapeDataString(database.FormatIdentifier()) + "&");
            }

            if (retentionPolicy != null)
            {
                queryString.Append("rp=" + Uri.EscapeDataString(retentionPolicy.FormatIdentifier()) + "&");
            }

            if (!string.IsNullOrEmpty(Settings.Username) && !string.IsNullOrEmpty(Settings.Password))
            {
                queryString.Append("u=" + Uri.EscapeDataString(Settings.Username.Trim()) + "&");
                queryString.Append("p=" + Uri.EscapeDataString(Settings.Password.Trim()) + "&");
            }

            if (timePrecision.HasValue)
            {
                switch (timePrecision)
                {
                    case TimePrecision.Nanosecond:
                        // This is the default precision
                        queryString.Append("precision=n&");
                        break;
                    case TimePrecision.Microsecond:
                        queryString.Append("precision=u&");
                        break;
                    case TimePrecision.Millisecond:
                        queryString.Append("precision=ms&");
                        break;
                    case TimePrecision.Second:
                        queryString.Append("precision=s&");
                        break;
                    case TimePrecision.Minute:
                        queryString.Append("precision=m&");
                        break;
                    case TimePrecision.Hour:
                        queryString.Append("precision=h&");
                        break;
                }
            }

            if (consistency.HasValue)
            {
                switch (consistency)
                {
                    case Consistency.One:
                        queryString.Append("consistency=one&");
                        break;
                    case Consistency.Quorum:
                        queryString.Append("consistency=quorum&");
                        break;
                    case Consistency.All:
                        queryString.Append("consistency=all&");
                        break;
                    case Consistency.Any:
                        queryString.Append("consistency=any&");
                        break;
                }
            }

            if (resultPrecision.HasValue)
            {
                switch (resultPrecision.Value)
                {
                    case TimePrecision.Nanosecond:
                        // This is the default precision
                        queryString.Append("epoch=n&");
                        break;
                    case TimePrecision.Microsecond:
                        queryString.Append("epoch=u&");
                        break;
                    case TimePrecision.Millisecond:
                        queryString.Append("epoch=ms&");
                        break;
                    case TimePrecision.Second:
                        queryString.Append("epoch=s&");
                        break;
                    case TimePrecision.Minute:
                        queryString.Append("epoch=m&");
                        break;
                    case TimePrecision.Hour:
                        queryString.Append("epoch=h&");
                        break;
                }
            }

            if (queryString.Length > 1)
            {
                queryString.Length -= 1;
            }
            else
            {
                queryString.Length = 0;
            }

            return string.Format("{0}://{1}:{2}{3}{4}", 
                Settings.UseHttps ? "https" : "http", 
                Settings.Host, 
                Settings.Port,
                path.StartsWith("/") ? path : "/" + path,
                queryString);
        }


        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) { }
    }
}