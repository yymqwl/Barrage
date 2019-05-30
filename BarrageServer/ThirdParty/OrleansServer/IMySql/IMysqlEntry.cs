using Orleans;
using System;
using System.Threading.Tasks;

namespace IMySql
{
    public interface IMysqlEntry: IGrainWithIntegerKey
    {
        Task<string> GetName();

    }
}
