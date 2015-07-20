using System.Diagnostics.CodeAnalysis;

namespace InfluxDB
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class User
    {
        public string Username { get; private set; }
        public bool IsClusterAdmin { get; private set; }

        internal User(string username, bool isClusterAdmin)
        {
            Username = username;
            IsClusterAdmin = isClusterAdmin;
        }
    }
}