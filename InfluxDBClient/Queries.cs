using InfluxDB.Enums;
using InfluxDB.Extensions;

namespace InfluxDB
{
    internal static class Queries
    {
        private const string CreateDatabaseQueryFormat = "CREATE DATABASE {0}";
        private const string DropDatabaseQueryFormat = "CREATE DATABASE {0}";
        private const string ShowDatabasesQueryFormat = "SHOW DATABASES";
        private const string CreateRetentionPolicyQueryFormat = "CREATE RETENTION POLICY {0} ON {1} DURATION {2} REPLICATION {3}{4}";
        private const string ShowRetentionPoliciesQueryFormat = "SHOW RETENTION POLICIES {0}";
        private const string AlterRetentionPolicyQueryFormat = "ALTER RETENTION POLICY {0} ON {1}{2}{3}{4}";
        private const string CreateUserQueryFormat = "CREATE USER {0} WITH PASSWORD '{1}'{2}";
        private const string SetUserPasswordQueryFormat = "SET PASSWORD FOR {0} = '{1}'";
        private const string ShowUsersQueryFormat = "SHOW USERS";
        private const string DropUserQueryFormat = "DROP USER {0}";
        private const string GrantPrivilegeQueryFormat = "GRANT {0} ON {1} TO {2}";
        private const string RevokePrivilegeQueryFormat = "REVOKE {0} ON {1} FROM {2}";
        private const string RevokeClusterAdminPrivilegeQueryFormat = "REVOKE ALL PRIVILEGES FROM {0}";

        public static string GetCreateDatabaseQuery(string databaseName)
        {
            return string.Format(CreateDatabaseQueryFormat, databaseName.FormatIdentifier());
        }

        public static string GetDropDatabaseQuery(string databaseName)
        {
            return string.Format(DropDatabaseQueryFormat, databaseName.FormatIdentifier());
        }

        public static string GetShowDatabasesQuery()
        {
            return ShowDatabasesQueryFormat;
        }

        public static string GetCreateRetentionPolicyQuery(string policyName, string databaseName, Retention duration, int replication, bool isDefault = false)
        {
            return string.Format(
                CreateRetentionPolicyQueryFormat,
                policyName.FormatIdentifier(),
                databaseName.FormatIdentifier(),
                duration,
                replication,
                (isDefault ? " DEFAULT" : string.Empty)
            );
        }

        public static string GetShowRetentionPoliciesQuery(string databaseName)
        {
            return string.Format(
                ShowRetentionPoliciesQueryFormat,
                databaseName.FormatIdentifier()
            );
        }

        public static string GetAlterRetentionPoliciesQuery(string retentionPolicyName, string databaseName, Retention duration = null, int? replication = null, bool? isDefault = false)
        {
            return string.Format(
                AlterRetentionPolicyQueryFormat,
                retentionPolicyName.FormatIdentifier(),
                databaseName.FormatIdentifier(),
                (duration != null ? " DURATION " + duration : string.Empty),
                (replication.HasValue ? " REPLICATION " + replication.Value : string.Empty),
                (isDefault.HasValue ? " DEFAULT" : string.Empty)
            );
        }

        public static string GetCreateUserQuery(string username, string password, bool isClusterAdmin)
        {
            // TODO: Check escaping rules
            return string.Format(
                CreateUserQueryFormat,
                username.FormatIdentifier(),
                password.Replace("'", "\\'"),
                (isClusterAdmin ? " WITH ALL PRIVILEGES" : "")
            );
        }

        public static string GetSetUserPasswordQuery(string username, string password)
        {
            // TODO: Check escaping rules
            return string.Format(
                SetUserPasswordQueryFormat,
                username.FormatIdentifier(),
                password.Replace("'", "\\'")
            );
        }

        public static string GetShowUsersQuery()
        {
            return ShowUsersQueryFormat;
        }

        public static string GetDropUserQuery(string username)
        {
            return string.Format(
                DropUserQueryFormat,
                username.FormatIdentifier()
            );
        }

        public static string GetGrantPrivilegeQuery(string databaseName, string username, Privilege privilege)
        {
            return string.Format(
                GrantPrivilegeQueryFormat,
                privilege.ToString().ToUpper(),
                databaseName.FormatIdentifier(),
                username.FormatIdentifier()
            );
        }

        public static string GetRevokePrivilegeQuery(string databaseName, string username, Privilege privilege)
        {
            return string.Format(
                RevokePrivilegeQueryFormat,
                privilege.ToString().ToUpper(),
                databaseName.FormatIdentifier(),
                username.FormatIdentifier()
            );
        }

        public static string GetRevokeClusterAdminPrivilegeQuery(string username)
        {
            return string.Format(
                RevokeClusterAdminPrivilegeQueryFormat,
                username.FormatIdentifier()
            );
        }
    }
}