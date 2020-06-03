using GameFramework;
using GameFramework.Fsm;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class GTableRoom : Grain, ITableRoom
    {
        protected TableRoomInfo m_TableRoomInfo;

        protected IMainEntry m_GMainEntry;
        protected INetUserEntry m_INetUserEntry;
        protected List<TableUser_Data> m_Need_Del = new List<TableUser_Data>();

        
        public ETableRoomState TableRoomState
        {
            get;
            set;
        }

        Fsm<GTableRoom> m_Fsm;
        public Fsm<GTableRoom> Fsm
        {
            get { return m_Fsm; }
        }



        public async Task<bool> Init()
        {
            m_GMainEntry = GrainFactory.GetGrain<IMainEntry>(0);
            m_INetUserEntry = await m_GMainEntry.GetINetUserEntry();

            m_TableRoomInfo = new TableRoomInfo();

            m_TableRoomInfo.TableRoomState = ETableRoomState.ERoom_InActive;
            m_TableRoomInfo.IMaxRoomPlayer = GameConstant.IMaxTableRoomPlayer;
            m_TableRoomInfo.Ls_User.Clear();
            m_TableRoomInfo.Id = this.GetPrimaryKeyString();


           
            {
                TableRoomState = ETableRoomState.ERoom_InActive;
                var tr_idle = new TR_Idle();
                var tr_ready = new TR_Ready();
                var tr_loading = new TR_Loading();
                var tr_ingame = new TR_InGame();
                var tr_gameover = new TR_GameOver();
                var tr_inactive = new TR_InActive();
                m_Fsm = new Fsm<GTableRoom>(this.GetType().FullName, this,
                    tr_inactive, tr_idle, tr_ready, tr_loading, tr_ingame, tr_gameover);
                m_Fsm.Start<TR_InActive>();
            }

            var bret = true;
            return bret;//Task.FromResult(bret);
        }

        public Task<bool> ShutDown()
        {
            m_TableRoomInfo.Ls_User.Clear();
            m_TableRoomInfo.TableRoomState = ETableRoomState.ERoom_InActive;
            var bret = true;
            return Task.FromResult(bret);
        }
        public bool HasPlayer(string  id)
        {
            var bret = false;
            foreach(var user in m_TableRoomInfo.Ls_User)
            {
                if(user.Id == id)
                {
                    bret =  true;
                    break;
                }
            }
            return bret;
        }
        public TableUser_Data GetPlayer_Data(string id)
        {

            foreach (var user in m_TableRoomInfo.Ls_User)
            {
                if (user.Id == id)
                {
                    return user;
                }
            }
            return null;
        }
        public bool CanJoin()
        {
            if(this.m_TableRoomInfo.Ls_User.Count < this.m_TableRoomInfo.IMaxRoomPlayer)
            {
                return true;
            }
            return false;
        }
        public  Task<int> Join(TableUser_Data user)
        {
            var iret = 1;
            if(!CanJoin())
            {
                iret = -2;//人数已满
                return Task.FromResult(iret);
            }
            if(HasPlayer(user.Id))
            {
                iret =  2;//已加入
                return Task.FromResult(iret);
            }

            m_TableRoomInfo.Ls_User.Add(user);
            return Task.FromResult(iret);

        }
        public bool RemovePlayer(string id)
        {
            var tu_data = m_TableRoomInfo.Ls_User.Find((TableUser_Data tu_d) =>
            {
                if(tu_d.Id == id)
                {
                    return true;
                }
                return false;
            });
            if(tu_data == null)
            {
                return false;
            }
            m_TableRoomInfo.Ls_User.Remove(tu_data);
            return true;
        }
        public Task<int> Exit(string id)
        {
            var iret = 1;
            if (!HasPlayer(id))
            {
                iret = -2;//不再房间内
                return Task.FromResult(iret);
            }
            RemovePlayer(id);
            Log.Debug($"{id}退出房间:{m_TableRoomInfo.Id}");
            //
            return Task.FromResult(iret);
        }

        public async Task SendToAllPlayer(byte[] msg)
        {
            foreach(var tu_data in m_TableRoomInfo.Ls_User)
            {
               await  m_INetUserEntry.SendMessage(tu_data.Id, msg);
            }
        }

        //检查不再活跃的用户
        public async Task CheckInActivePlayer()
        {
            m_Need_Del.Clear();
            foreach (var tu_data in m_TableRoomInfo.Ls_User )
            {
                var nu =  await m_INetUserEntry.GetINetUser(tu_data.Id);
                if(! await nu.GetIsConnected())
                {
                    m_Need_Del.Add(tu_data);
                }
            }
            foreach(var tu_data in m_Need_Del)
            {
                await Exit(tu_data.Id);
            }
            if(m_TableRoomInfo.Ls_User.Count == 0)
            {

            }

        }
        


        public Task Update(float t)
        {
            if(m_Fsm!=null)
            {
                m_Fsm.Update(t, t);
            }
            return Task.CompletedTask;
        }
    }
}
