namespace InfluxDB
{
    public class User
    {
        public string Username { get; private set; }
        public bool IsAdmin { get; private set; }

        internal User(string username, bool isAdmin)
        {
            Username = username;
            IsAdmin = isAdmin;
        }
    }
}