using System.Diagnostics.CodeAnalysis;

namespace InfluxDB
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class InfluxServerInfo
    {
        public long Id { get; set; }

        public string ProtobufConnectString { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, ProtobufConnectString: {1}", Id, ProtobufConnectString);
        }
    }
}