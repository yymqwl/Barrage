using GameFramework;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class GTableRoomEntry : GEntry, ITableRoomEntry
    {
        Dictionary<string, ITableRoom> m_Dict_TableRoom = new Dictionary<string, ITableRoom>();


        public  override Task<bool> Init()
        {
            var bret = true;
            Log.Debug("GTableRoomEntry Init" + this.GetPrimaryKeyLong());
            m_Dict_TableRoom.Clear();
            //m_Dict_TableUser.Clear();
            return Task.FromResult(bret);
        }
        public override Task<bool> ShutDown()
        {
            var bret = base.ShutDown();
            Log.Debug("GTableRoomEntry ShutDown" + this.GetPrimaryKeyLong());

            return bret;
        }

        /*
        Dictionary<string, ITableUser> m_Dict_TableUser = new Dictionary<string, ITableUser>();
        public ITableUser GetITableUser(string id)
        {
            ITableUser icu = default;
            m_Dict_TableUser.TryGetValue(id, out icu);
            return icu;
        }
        public ITableUser CreateITableUser(TableUser_Data tableUser_Data)
        {
            var itu = this.GrainFactory.GetGrain<ITableUser>(tableUser_Data.Id);
            m_Dict_TableUser.TryAdd(tableUser_Data.Id, itu);
            await itu.SetChatUser_Data(user);
            return cu;
        }*/


        public async Task<int> Exit(TableUser_Data user)
        {
            var iret = 1;

            if (user.RoomId == string.Empty)
            {
                iret = 2;
                return iret;
            }
            ITableRoom tr = GetTableRoom(user.RoomId);
            if(tr == null)
            {
                iret = 3;
                return iret;
            }
            iret = await tr.Exit(user.Id);

            return iret;
        }
        public string GenerateId()
        {
            return IdStringGenerater.GenerateIdString(8);
        }
        public async Task<ITableRoom> CreateTableRoom()
        {
            var tr= GrainFactory.GetGrain<ITableRoom>(GenerateId());
            await tr.Init();
            return tr;
        }
        public ITableRoom GetTableRoom(string id)
        {
            ITableRoom itr = default;
            m_Dict_TableRoom.TryGetValue(id, out itr);
            return itr;
        }
        public async Task<int> Join(TableUser_Data user)
        {
            var iret = 1;
            ITableRoom tr = null;
            if(user.RoomId == string.Empty)
            {
                tr = await CreateTableRoom();
            }
            else
            {
                tr = GetTableRoom(user.RoomId);
                if(tr!=null)
                {
                    iret = -2;//已经在房间中
                    return iret;
                }
                else
                {
                    tr = await CreateTableRoom();
                }
            }

            iret = await tr.Join(user);

            return iret;
        }
        public override Task Update(float t)
        {

            return Task.CompletedTask;
        }

    }
}
