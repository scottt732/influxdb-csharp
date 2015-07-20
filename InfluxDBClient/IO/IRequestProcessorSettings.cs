namespace InfluxDB.IO
{
    public interface IRequestProcessorSettings
    {
        string Host { get; }
        int Port { get; }
        bool UseHttps { get; }
        string Username { get; }
        string Password { get; }
    }
}