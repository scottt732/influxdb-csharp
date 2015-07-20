using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Enums;
using InfluxDB.Extensions;
using InfluxDB.IO;
using InfluxDB.Request;
using InfluxDB.Response;
using InfluxDB.Timestamp;

namespace InfluxDB
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class InfluxDBClient : IInfluxDbClient
    {
        public IRequestProcessor RequestProcessor { get; private set; }
        public ITimestampGenerator TimestampGenerator { get; private set; }

        public string Database { get; private set; }
        public Consistency DefaultConsistency { get; private set; }

        private InfluxDBClient(string database, Consistency defaultConsistency)
        {
            Database = database;
            DefaultConsistency = defaultConsistency;
        }

        public InfluxDBClient(IRequestProcessor requestProcessor, ITimestampGenerator timestampGenerator, string database, Consistency defaultConsistency) : this(database, defaultConsistency)
        {
            RequestProcessor = requestProcessor;
            TimestampGenerator = timestampGenerator;
        }

        public InfluxDBClient(string host, int port, bool useHttps, string database, ITimestampGenerator timestampGenerator, Consistency defaultConsistency) : this(database, defaultConsistency)
        {
            RequestProcessor = new HttpClientRequestProcessor(new RequestProcessorSettings(host, port, useHttps));
            TimestampGenerator = timestampGenerator;
        }

        public InfluxDBClient(string host, int port, bool useHttps, string username, string password, string database, ITimestampGenerator timestampGenerator, Consistency defaultConsistency) : this(database, defaultConsistency)
        {
            RequestProcessor = new HttpClientRequestProcessor(new RequestProcessorSettings(host, port, useHttps, username, password));
            TimestampGenerator = timestampGenerator;
        }

        public InfluxDBClient(string host, int port, bool useHttps, string database, Consistency defaultConsistency, TimePrecision timestampPrecision = TimePrecision.Nanosecond) : this(database, defaultConsistency)
        {
            RequestProcessor = new HttpClientRequestProcessor(new RequestProcessorSettings(host, port, useHttps));
            TimestampGenerator = new AdaptiveTimestampGenerator(timestampPrecision);
        }

        public InfluxDBClient(string host, int port, bool useHttps, string username, string password, string database, Consistency defaultConsistency, TimePrecision timestampPrecision = TimePrecision.Nanosecond) : this(database, defaultConsistency)
        {
            RequestProcessor = new HttpClientRequestProcessor(new RequestProcessorSettings(host, port, useHttps, username, password));
            TimestampGenerator = new AdaptiveTimestampGenerator(timestampPrecision);
        }

        public async Task WriteMessage(WriteMessage message, Consistency? consistency = null, string retentionPolicy = null)
        {
            await RequestProcessor.WriteMessage(Database, message, consistency ?? DefaultConsistency, retentionPolicy, TimestampGenerator.GetTimestamp(), TimestampGenerator.Precision).ConfigureAwait(false);
        }

        public async Task WriteMessages(IEnumerable<WriteMessage> messages, Consistency? consistency = null, string retentionPolicy = null)
        {
            await RequestProcessor.WriteMessages(Database, messages, consistency ?? DefaultConsistency, retentionPolicy, TimestampGenerator.GetTimestamp(), TimestampGenerator.Precision).ConfigureAwait(false);
        }

        public async Task<ResultSet> Query(string query, TimePrecision resultPrecision = TimePrecision.Microsecond, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await RequestProcessor.SendQuery(query, resultPrecision, cancellationToken).ConfigureAwait(false);
        }

        public async Task CreateDatabase(string databaseName)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetCreateDatabaseQuery(databaseName), 
                CancellationToken.None
            ).ConfigureAwait(false);            
            resultSet.EnsureEmptySuccessResult();            
        }

        public async Task DropDatabase(string databaseName)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetDropDatabaseQuery(databaseName), 
                CancellationToken.None
            ).ConfigureAwait(false);
            resultSet.EnsureEmptySuccessResult();
        }

        public async Task<string[]> GetDatabases(CancellationToken cancellationToken = default(CancellationToken))
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetShowDatabasesQuery(), 
                CancellationToken.None
            ).ConfigureAwait(false);            
            return resultSet.ToSimpleStringList();
        }

        public async Task CreateRetentionPolicy(string policyName, string databaseName, Retention duration, int replication, bool isDefault = false)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetCreateRetentionPolicyQuery(
                    policyName,
                    databaseName,
                    duration,
                    replication,
                    isDefault
                ),
                CancellationToken.None
            ).ConfigureAwait(false);
            resultSet.EnsureEmptySuccessResult();
        }

        public async Task<RetentionPolicy[]> GetRetentionPolicies(string databaseName)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetShowRetentionPoliciesQuery(databaseName),
                CancellationToken.None
            ).ConfigureAwait(false);

            return resultSet.ToRetentionPolicies();
        }

        public async Task AlterRetentionPolicy(string retentionPolicyName, string databaseName, Retention duration = null, int? replication = null, bool? isDefault = false)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetAlterRetentionPoliciesQuery(retentionPolicyName, databaseName, duration, replication, isDefault),
                CancellationToken.None
            ).ConfigureAwait(false);
            resultSet.EnsureEmptySuccessResult();
        }

        public async Task CreateUser(string username, string password, bool isClusterAdmin)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetCreateUserQuery(username, password, isClusterAdmin),
                CancellationToken.None
            ).ConfigureAwait(false);
            resultSet.EnsureEmptySuccessResult();
        }

        public async Task SetUserPassword(string username, string password)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetSetUserPasswordQuery(username, password),
                CancellationToken.None
            ).ConfigureAwait(false);
            resultSet.EnsureEmptySuccessResult();
        }

        public async Task<User[]> GetUsers(CancellationToken cancellationToken = default(CancellationToken))
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetShowUsersQuery(),
                CancellationToken.None
            ).ConfigureAwait(false);
            return resultSet.ToUsers();
        }

        public async Task DeleteUser(string username)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetDropUserQuery(username),
                CancellationToken.None
            ).ConfigureAwait(false);
            resultSet.EnsureEmptySuccessResult();
        }

        public async Task GrantPrivilege(string databaseName, string username, Privilege privilege)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetGrantPrivilegeQuery(databaseName, username, privilege),
                CancellationToken.None
            ).ConfigureAwait(false);
            resultSet.EnsureEmptySuccessResult();
        }

        public async Task RevokePrivilege(string databaseName, string username, Privilege privilege)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetRevokePrivilegeQuery(databaseName, username, privilege),
                CancellationToken.None
            ).ConfigureAwait(false);
            resultSet.EnsureEmptySuccessResult();
        }

        public async Task RevokeClusterAdminPrivilege(string username)
        {
            var resultSet = await RequestProcessor.SendQuery(
                Queries.GetRevokeClusterAdminPrivilegeQuery(username),
                CancellationToken.None
            ).ConfigureAwait(false);
            resultSet.EnsureEmptySuccessResult();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RequestProcessor.Dispose();
            }          
        }
    }

    /*
    public void DeleteSerie(string name)
    {
        Query("DROP SERIES " + name);
    }

    public List<InfluxServerInfo> GetServers()
    {
        var result = new List<InfluxServerInfo>();
        var url = CreateUrl("/cluster/servers");
        var res = (IEnumerable<dynamic>) Request(url, "GET");

        if (res != null)
        {
            result.AddRange(res.Select(server => new InfluxServerInfo {Id = server.id.Value, ProtobufConnectString = server.protobufConnectString.Value}));
        }

        return result;
    }

    public void Insert(string database, IList<Series> series)
    {
        var url = CreateUrl("/db/" + database + "/series");

        //TODO options
        Request(url, "POST", series);
    }

    public List<Series> Query(string database, string query, TimePrecision? precision = null)
    {
        var escapedQuery = HttpUtility.UrlEncode(query);
        var url = CreateUrl("/db/" + database + "/series");
        if (precision.HasValue)
        {
            url += "&time_precision=" + _timePrecision[(int) precision.Value];
        }
        url += "&q=" + escapedQuery;
        var result = Request(url, "GET");

        if (result != null)
        {
            return JsonConvert.DeserializeObject<List<Series>>(result.ToString());
        }

        return null;
    }

    public bool Ping()
    {
        var url = CreateUrl("/ping");
        dynamic response = Request(url, "GET");

        return response.status == "ok";
    }
    */
}