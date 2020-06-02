using GameFramework;
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


        protected bool m_BInit = false;
        protected bool m_BShutDown = false;
        public async override Task<bool> Init()
        {
            var bret = true;
            if (!m_BInit)
            {
                m_BInit = true;
                await AddIEntry(await GetIChatRoomEntry());
                bret = await base.Init();
            }
            return  bret;
        }

        public async override Task<bool> ShutDown()
        {
            var bret = true;
            if(!m_BShutDown)
            {
                m_BShutDown = true;
                bret = await base.ShutDown();
            }
            return bret;
        }

        public override Task OnActivateAsync()
        {
            Log.Debug("OnActivateAsync:GMainEntry");
            return base.OnActivateAsync();
        }
        public override Task OnDeactivateAsync()
        {
            Log.Debug("OnDeactivateAsync:GMainEntry");
            return base.OnDeactivateAsync();
        }


    }
}
