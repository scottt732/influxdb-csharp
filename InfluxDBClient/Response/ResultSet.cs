using System.Diagnostics.CodeAnalysis;

namespace InfluxDB.Response
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ResultSet
    {
        public Result[] Results { get; set; }
        
        // e.g., Invalid authentication credentials
        public string Error { get; set; }
    }
}