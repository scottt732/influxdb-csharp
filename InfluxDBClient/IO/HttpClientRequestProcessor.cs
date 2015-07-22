using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Enums;
using InfluxDB.Extensions;
using InfluxDB.Request;
using InfluxDB.Response;

namespace InfluxDB.IO
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class HttpClientRequestProcessor : BaseRequestProcessor
    {
        private readonly HttpClient _client;

        public HttpClientRequestProcessor(IRequestProcessorSettings settings) : base(settings)
        {
            _client = new HttpClient();
        }

        public HttpClientRequestProcessor(IRequestProcessorSettings settings, HttpMessageHandler httpMessageHandler) : base(settings)
        {
            _client = new HttpClient(httpMessageHandler);
        }

        public HttpClientRequestProcessor(IRequestProcessorSettings settings, HttpMessageHandler httpMessageHandler, bool disposeHandler) : base(settings)
        {
            _client = new HttpClient(httpMessageHandler, disposeHandler);
        }

        public override async Task<ResultSet> SendQuery(string database, string query, TimePrecision resultPrecision = TimePrecision.Microsecond, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var message = new HttpRequestMessage(HttpMethod.Post, CreateUrl("/query", database, query)))
            using (var response = await _client.SendAsync(message, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false))
            {
                response.ValidateHttpResponse(HttpStatusCode.OK, false);

                var result = await response.Content
                    .ReadAsAsync<ResultSet>(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            }
        }

        public override async Task SendCommand(string command)
        {
            await SendCommand(null, command).ConfigureAwait(false);
        }

        public override async Task SendCommand(string database, string command)
        {
            using (var message = new HttpRequestMessage(HttpMethod.Post, CreateUrl("/query", database, command)))
            using (var response = await _client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false))
            {
                response.ValidateHttpResponse(HttpStatusCode.OK, false);
            }
        }

        public override async Task WriteMessage(string database, WriteMessage message, Consistency? consistency = null, string retentionPolicy = null, DateTime? timestamp = null, TimePrecision? precision = null)
        {
            using (var msg = new HttpRequestMessage(HttpMethod.Post, CreateUrl("/write", database, null, null, consistency, retentionPolicy)))
            {
                msg.Content = new StringContent(message.ToString(timestamp, precision), Encoding.UTF8);
                using (var response = await _client.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false))
                {
                    response.ValidateHttpResponse(HttpStatusCode.NoContent, true);
                }
            }
        }

        public override async Task WriteMessages(string database, IEnumerable<WriteMessage> messages, Consistency? consistency = null, string retentionPolicy = null, DateTime? timestamp = null, TimePrecision? precision = null)
        {
            using (var msg = new HttpRequestMessage(HttpMethod.Post, CreateUrl("/write", database, null, null, consistency, retentionPolicy)))
            {
                msg.Content = new StringContent(string.Join("\n", messages.Select(x => x.ToString(timestamp, precision))), Encoding.UTF8);
                using (var response = await _client.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None).ConfigureAwait(false))
                {
                    response.ValidateHttpResponse(HttpStatusCode.NoContent, true);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client.Dispose();
            }
        }
    }
}