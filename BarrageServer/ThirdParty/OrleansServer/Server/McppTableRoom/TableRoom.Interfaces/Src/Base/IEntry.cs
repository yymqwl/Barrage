using Orleans;
using System;
using System.Threading.Tasks;

namespace TableRoom
{
    //无状态
    public interface IEntry : IGrainWithIntegerKey
    {
        public Task<int> GetPriority();
        Task<bool> Init();
        Task Update(float t);
        Task<bool> ShutDown();
    }
}
