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
        protected LinkedList<string> m_NeedDel_TableRoom = new LinkedList<string>();
        protected UpdateTime m_Update_TableRoom;
        Dictionary<string, string> m_Dict_User_Room = new Dictionary<string, string>();//一个角色Id对应一个房间Id
        
        public string GetUserRoomId(string id)
        {
            string roomid = string.Empty;
            m_Dict_User_Room.TryGetValue(id,out roomid);
            return roomid;
        }
        public void SetUserRoomId(string userid,string roomid)
        {
            m_Dict_User_Room[userid] = roomid;
        }
        public bool RemoveUserId(string id)
        {
             return m_Dict_User_Room.Remove(id);
        }
        public void SetTableRoom( string roomid, ITableRoom  itr)
        {
            m_Dict_TableRoom[roomid] = itr;
        }


        public  override Task<bool> Init()
        {
            var bret = true;
            Log.Debug("GTableRoomEntry Init" + this.GetPrimaryKeyLong());
            m_Dict_TableRoom.Clear();

            m_Update_TableRoom = new UpdateTime(GameConstant.TCheckNetUser);
            m_Update_TableRoom.Evt_Act += Update_TableRoom;

            return Task.FromResult(bret);
        }
        public override Task<bool> ShutDown()
        {
            m_Update_TableRoom.Evt_Act -= Update_TableRoom;
            m_Update_TableRoom = null;
            var bret = base.ShutDown();
            Log.Debug("GTableRoomEntry ShutDown" + this.GetPrimaryKeyLong());
            return bret;
        }

        public async void Update_TableRoom()
        {
            m_NeedDel_TableRoom.Clear();
            foreach (var vk in  m_Dict_TableRoom)
            {
                await vk.Value.CheckInActivePlayer();
                if(await vk.Value.NeedClearUp())
                {
                    await vk.Value.ShutDown();
                    m_NeedDel_TableRoom.AddLast(vk.Key);
                }
            }

            
            foreach (var idstr in m_NeedDel_TableRoom)
            {
                m_Dict_TableRoom.Remove(idstr);
                Log.Debug($"CleanUpTableRoom{idstr}");
            }
        }
        


        public async Task<int> Exit(string id)
        {
            var iret = 1;

            var roomid = GetUserRoomId(id);
            if(roomid == null)
            {
                return iret;
            }

            //var tu_data = await tu.GetTableUser_Data();

            ITableRoom tr = GetTableRoom(roomid );
            if(tr == null)
            {
                iret = 3;
                return iret;
            }
            iret = await tr.Exit(id);

            if (iret > 0)
            {
                roomid = string.Empty;
                this.SetUserRoomId(id, roomid);
            }

            return iret;
        }
        public string GenerateId()
        {
            return IdStringGenerater.GenerateIdString(8);
        }
        public async Task<ITableRoom> CreateTableRoom()
        {
            var roomid = GenerateId();
            var tr= GrainFactory.GetGrain<ITableRoom>(roomid);
            await tr.Init();
            SetTableRoom(roomid, tr);

            return tr;
        }
        public ITableRoom GetTableRoom(string id)
        {
            ITableRoom itr = default;
            m_Dict_TableRoom.TryGetValue(id, out itr);
            return itr;
        }
        public async Task<int> Join(TableUser_Data user_data)
        {
            var iret = 1;
            ITableRoom tr = null;

            var roomid = GetUserRoomId(user_data.Id);
            if(roomid != null)
            {
                user_data.RoomId = roomid;
                //iret = -2;//已经在别的房间
                //return iret;
            }
            /*
            var tu_data = GetTableUser_Data(user_data.Id);  //this.GetITableUser(user_data.Id);
            if(tu_data == null)
            {
                AddTableUser_Data(user_data);
            }*/

            if(user_data.RoomId == string.Empty)
            {
                tr = await CreateTableRoom();

            }
            else
            {
                tr = GetTableRoom(user_data.RoomId);
                if(tr!=null)
                {
                    iret = 2;//已经在房间中
                    return iret;
                }
                else
                {
                    tr = await CreateTableRoom();
                }
            }

            iret = await tr.Join(user_data);
            if(iret  >0 )
            {
                var trinfo = await tr.GetTableRoomInfo();
                SetUserRoomId(user_data.Id, trinfo.Id);
            }

            return iret;
        }
        public override Task Update(float t)
        {
            this.m_Update_TableRoom.Update(t);
            return Task.CompletedTask;
        }

        public async Task<TableRoomInfo> GetUserTableRoomInfo(string id)
        {
            var roomid = GetUserRoomId(id);
            if(roomid == null)
            {
                return null;
            }
            var tr =  GetTableRoom(roomid);
            if (tr == null)
            {
                return null;
            }
            return await tr.GetTableRoomInfo();
        }

    }
}
