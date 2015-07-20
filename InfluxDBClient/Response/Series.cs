using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace InfluxDB.Response
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Series
    {
        public string Name { get; set; }
        public Dictionary<string, object> Tags { get; set; }
        public string[] Columns { get; set; }
        public object[][] Values { get; set; }
    }
}