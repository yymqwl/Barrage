using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public  class GMainEntry :GAMainEntry , IMainEntry
    {
        protected IChatRoomEntry m_IChatRoomEntry;
        public Task<IChatRoomEntry> GetIChatRoomEntry()
        {
            if (m_IChatRoomEntry == null)
            {
                m_IChatRoomEntry = GrainFactory.GetGrain<IChatRoomEntry>(0);
            }
            return Task.FromResult(m_IChatRoomEntry);
        }


        public async override Task<bool> Init()
        {
            await AddIEntry(await GetIChatRoomEntry());

            var bret = await base.Init();
            return  bret;
        }





    }
}
