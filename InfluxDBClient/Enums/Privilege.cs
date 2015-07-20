using System.Diagnostics.CodeAnalysis;

namespace InfluxDB.Enums
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum Privilege
    {
        Read,
        Write,
        All
    }
}
