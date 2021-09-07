using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public interface IServerTimer : IGrainWithIntegerKey
    {
        Task<DateTime> GetDateTimeNow();

        Task<float> DeltaTime();
        Task<bool> CheckUpdate();
    }
}
