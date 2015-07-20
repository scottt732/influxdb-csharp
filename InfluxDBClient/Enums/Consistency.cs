using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfluxDB.Enums
{
    public enum Consistency
    {
        One,
        Quorum,
        All,
        Any
    }
}
