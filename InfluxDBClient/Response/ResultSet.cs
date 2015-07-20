namespace InfluxDB.Response
{
    public class ResultSet
    {
        public Result[] Results { get; set; }
        
        // e.g., Invalid authentication credentials
        public string Error { get; set; }
    }
}