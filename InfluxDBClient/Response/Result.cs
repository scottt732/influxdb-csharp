using System.Diagnostics.CodeAnalysis;

namespace InfluxDB.Response
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Result
    {
        public Series[] Series { get; set; }

        // e.g., Attempt to query a series that does not exist
        public string Error { get; set; } 
    }
}