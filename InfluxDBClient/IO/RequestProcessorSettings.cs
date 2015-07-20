namespace InfluxDB.IO
{
    public class RequestProcessorSettings : IRequestProcessorSettings
    {
        public string Host { get; private set; }
        public int Port { get; private set; }
        public bool UseHttps { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public RequestProcessorSettings(string host, int port, bool useHttps)
        {
            Host = host;
            Port = port;
            UseHttps = useHttps;
        }

        public RequestProcessorSettings(string host, int port, bool useHttps, string username, string password) : this(host, port, useHttps)
        {
            Username = username;
            Password = password;
        }
    }
}