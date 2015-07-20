using System;
using System.Collections.Generic;
using System.Linq;
using InfluxDB.Response;

namespace InfluxDB.Extensions
{
    public static class ResultSetExtensions
    {
        public static object[] ToSimpleList(this ResultSet resultSet)
        {
            resultSet.EnsureSuccess();

            var result = resultSet.GetSingleResult();

            result.EnsureSuccess();

            var firstSeries = result.GetSingleSeries();

            return firstSeries.ToSimpleDictionary().Select(x => x.Value).ToArray();
        }

        public static string[] ToSimpleStringList(this ResultSet resultSet)
        {
            resultSet.EnsureSuccess();

            return resultSet.ToSimpleList().Select(x => x.ToString()).ToArray();
        }

        public static IReadOnlyDictionary<string, object> ToSimpleDictionary(this ResultSet resultSet)
        {
            resultSet.EnsureSuccess();

            var result = resultSet.GetSingleResult();

            result.EnsureSuccess();

            var firstSeries = result.GetSingleSeries();

            return firstSeries.ToSimpleDictionary();
        }

        public static IReadOnlyDictionary<string, List<object>> ToMultiValuedDictionary(this ResultSet resultSet)
        {
            resultSet.EnsureSuccess();
            
            var result = resultSet.GetSingleResult();

            result.EnsureSuccess();

            var firstSeries = result.GetSingleSeries();

            return firstSeries.ToMultiValuedDictionary();
        }

        public static RetentionPolicy[] ToRetentionPolicies(this ResultSet resultSet)
        {
            var result = resultSet.ToMultiValuedDictionary();

            List<object> policyNameObjects;
            List<object> durationObjects;
            List<object> replicationObjects;

            if (!result.TryGetValue("name", out policyNameObjects) || !result.TryGetValue("duration", out durationObjects) || !result.TryGetValue("replicaN", out replicationObjects))
            {
                throw new InfluxException("The result set does not have the expected structure.");
            }

            if (policyNameObjects.Count != durationObjects.Count || durationObjects.Count != replicationObjects.Count)
            {
                throw new InfluxException("The result set is unexpectedly jagged.  This is likely an InfluxDB bug.");
            }

            var policyNames = policyNameObjects.Select(x => x.ToString()).ToArray();
            var durations = durationObjects.Select(x => Retention.Parse(x.ToString())).ToArray();
            var replications = replicationObjects.Select(x => int.Parse(x.ToString())).ToArray();

            return policyNames.Select((policyName, idx) => new RetentionPolicy(policyName, durations[idx], replications[idx])).ToArray();
        }

        public static User[] ToUsers(this ResultSet resultSet)
        {
            var result = resultSet.ToMultiValuedDictionary();

            List<object> usernameObjects;
            List<object> isAdminObjects;

            if (!result.TryGetValue("user", out usernameObjects) || !result.TryGetValue("admin", out isAdminObjects))
            {
                throw new InfluxException("The result set does not have the expected structure.");
            }

            if (usernameObjects.Count != isAdminObjects.Count)
            {
                throw new InfluxException("The result set is unexpectedly jagged.  This is likely an InfluxDB bug.");
            }

            var usernames = usernameObjects.Select(x => x.ToString()).ToArray();
            var isAdmins = isAdminObjects.Select(x => bool.Parse(x.ToString())).ToArray();

            return usernames.Select((username, idx) => new User(username, isAdmins[idx])).ToArray();
        }

        private static Result GetSingleResult(this ResultSet resultSet)
        {
            Result result;

            resultSet.EnsureSuccess();

            try
            {
                result = resultSet.Results.SingleOrDefault();

                if (result == null)
                {
                    throw new InfluxException("ResultSet did not contain any Results");
                }
            }
            catch (InvalidOperationException ioe)
            {
                throw new InfluxException("ResultSet contained an unexpected number of Results", ioe);
            }

            result.EnsureSuccess();
            
            return result;
        }

        public static void EnsureEmptySuccessResult(this ResultSet resultSet)
        {
            if (!string.IsNullOrEmpty(resultSet.Error))
            {
                throw new InfluxException(resultSet.Error);
            }
            if (resultSet.Results.Length != 0)
            {
                throw new InfluxException("Unexpected result.");
            }
        }

        public static void EnsureSuccess(this ResultSet resultSet)
        {
            if (!string.IsNullOrEmpty(resultSet.Error))
            {
                throw new InfluxException(resultSet.Error);
            }
        }
    }
}
