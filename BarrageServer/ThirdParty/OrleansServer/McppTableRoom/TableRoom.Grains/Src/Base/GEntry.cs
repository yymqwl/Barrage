using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class GEntry : Grain, IEntry
    {
        public virtual Task<int> GetPriority()
        {
            return Task.FromResult(0);
        }

        public virtual Task<bool> Init()
        {
            var bret = true;
            return Task.FromResult(bret);
        }

        public virtual Task<bool> ShutDown()
        {
            var bret = true;
            return Task.FromResult(bret);
        }

        public virtual Task Update(float t)
        {
            return Task.CompletedTask;
        }
    }
}
