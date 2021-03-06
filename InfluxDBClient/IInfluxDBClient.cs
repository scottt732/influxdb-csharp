using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Enums;
using InfluxDB.IO;
using InfluxDB.Request;
using InfluxDB.Response;
using InfluxDB.Timestamp;

namespace InfluxDB
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public interface IInfluxDbClient : IDisposable
    {
        [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
        IRequestProcessor RequestProcessor { get; }

        [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
        ITimestampGenerator TimestampGenerator { get; }

        [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
        string Database { get; }

        [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
        Consistency DefaultConsistency { get; }

        Task WriteMessage(WriteMessage message, Consistency? consistency = null, string retentionPolicy = null);
        Task WriteMessages(IEnumerable<WriteMessage> messages, Consistency? consistency = null, string retentionPolicy = null);
        Task<ResultSet> Query(string query, TimePrecision resultPrecision = TimePrecision.Microsecond, CancellationToken cancellationToken = default(CancellationToken));
        Task CreateDatabase(string databaseName);
        Task DropDatabase(string databaseName);

        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        Task<string[]> GetDatabases(CancellationToken cancellationToken = default(CancellationToken));
        Task CreateRetentionPolicy(string policyName, string databaseName, Retention duration, int replication, bool isDefault = false);
        Task<RetentionPolicy[]> GetRetentionPolicies(string databaseName);
        Task AlterRetentionPolicy(string retentionPolicyName, string databaseName, Retention duration = null, int? replication = null, bool? isDefault = false);
        Task CreateUser(string username, string password, bool isClusterAdmin);
        Task SetUserPassword(string username, string password);

        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        Task<User[]> GetUsers(CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteUser(string username);
        Task GrantPrivilege(string databaseName, string username, Privilege privilege);
        Task RevokePrivilege(string databaseName, string username, Privilege privilege);
        Task RevokeClusterAdminPrivilege(string username);
    }
}