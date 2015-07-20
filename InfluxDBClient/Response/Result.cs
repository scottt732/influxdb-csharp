namespace InfluxDB.Response
{
    public class Result
    {
        public Series[] Series { get; set; }

        // e.g., Attempt to query a series that does not exist
        public string Error { get; set; } 
    }
}