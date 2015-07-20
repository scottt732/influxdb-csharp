using System.Collections.Generic;

namespace InfluxDB.Response
{
    public class Series
    {
        public string Name { get; set; }
        public Dictionary<string, object> Tags { get; set; }
        public string[] Columns { get; set; }
        public object[][] Values { get; set; }
    }
}