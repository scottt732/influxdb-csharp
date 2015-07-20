using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Enums;
using InfluxDB.Request;
using InfluxDB.Response;

namespace InfluxDB.IO
{
    public interface IRequestProcessor : IDisposable
    {
        [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
        IRequestProcessorSettings Settings { get; }

        Task<ResultSet> SendQuery(string query, CancellationToken cancellationToken = default(CancellationToken));

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        Task<ResultSet> SendQuery(string database, string query, CancellationToken cancellationToken = default(CancellationToken));
        Task<ResultSet> SendQuery(string query, TimePrecision resultPrecision = TimePrecision.Microsecond, CancellationToken cancellationToken = default(CancellationToken));

        [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        Task<ResultSet> SendQuery(string database, string query, TimePrecision resultPrecision = TimePrecision.Microsecond, CancellationToken cancellationToken = default(CancellationToken));

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        Task SendCommand(string command);

        [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
        Task SendCommand(string database, string command);

        Task WriteMessage(string database, WriteMessage message, Consistency? consistency = null, string retentionPolicy = null, DateTime? timestamp = null, TimePrecision? precision = null);
        Task WriteMessages(string database, IEnumerable<WriteMessage> messages, Consistency? consistency = null, string retentionPolicy = null, DateTime? timestamp = null, TimePrecision? precision = null);
    }
}
