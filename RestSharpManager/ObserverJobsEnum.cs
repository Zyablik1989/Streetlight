using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpManager
{
    public enum ObserverJobsEnum
    {
        Idle = 0,
        Cancel = 1,
        GetSequenceCode = 2,
        SendSegmentsScheme = 3,
        ClearServerData = 4,
    }
}
