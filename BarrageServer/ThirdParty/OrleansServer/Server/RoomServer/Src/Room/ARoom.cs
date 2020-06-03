using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    public abstract class ARoom
    {
        protected string m_RoomId;//所在的房间Id
        public virtual string RoomId
        {
            get
            {
                return this.m_RoomId;
            }
        }

        bool m_IsAvailable;
        public bool IsAvailable
        {
            get { return m_IsAvailable; }
            set { m_IsAvailable = value; }
        }



        protected Dictionary<string,WebPlayer> m_Dict_Rp = new Dictionary<string, WebPlayer>();
        public Dictionary<string, WebPlayer> Dict_Rp
        {
            get
            {
                return m_Dict_Rp;
            }
        }

        protected uint m_MaxRoomPlayer;
        public uint MaxRoomPlayer
        {
            get { return m_MaxRoomPlayer; }
        }
        public virtual bool Init(string roomid)
        {
            this.m_RoomId = roomid;
            return true;
        }
        public virtual bool ShutDown()
        {
            this.m_Dict_Rp.Clear();
            return true;
        }

        public virtual bool HasPlayer(string id)
        {
            if(this.m_Dict_Rp.ContainsKey(id))
            {
                return true;
            }
            return false;
        }
        public WebPlayer GetPlayer(string id)
        {
            WebPlayer rp;
            this.m_Dict_Rp.TryGetValue(id,out rp);
            return rp;
        }
        public bool RemovePlayer(string id)
        {
            var wp = GetPlayer(id);
            if(wp!= null)
            {
                var rpb = wp.GetIBehaviour<RoomPlayerBv>();
                rpb.RoomId = string.Empty;
            }
            return this.m_Dict_Rp.Remove(id);
        }

        public void SendToAllPlayer(byte[] data)
        {
            foreach(var rp in m_Dict_Rp)
            {
                rp.Value.SendAsync(data);
            }
        }
        public int EnterRoom(WebPlayer webPlayer)
        {
            var ret = -1;
            var py = this.GetPlayer(webPlayer.Id);
            if (py != null)
            {
                ret = 2;//已经在房间中
                return ret;
            }
            //广播加入
            var rpd = webPlayer.GetIBehaviour<RoomPlayerBv>();
            rpd.RoomId = this.RoomId;
            rpd.RoomPlayer_Data.RoomId = this.RoomId;
            SendToAllPlayer(Msg_Json.Create_Msg_Json(NetOpCode.RoomPlayerJoin_Msg, new RoomPlayerJoin_Msg { RoomPlayer_Data = rpd.RoomPlayer_Data }));
            

            Dict_Rp.Add(webPlayer.Id, webPlayer);
            Log.Debug($"{webPlayer.Id}加入房间:{RoomId}");

            ret = 1;
            return ret;
        }
        public int ExitRoom(WebPlayer webPlayer)
        {
            var ret = -1;
            var py = this.GetPlayer(webPlayer.Id);
            if(py == null)
            {
                return ret;
            }

            ret = 1;
            RemovePlayer(webPlayer.Id);
            Log.Debug($"{webPlayer.Id}退出房间:{RoomId}");
            var rpd = webPlayer.GetIBehaviour<RoomPlayerBv>();
            SendToAllPlayer(Msg_Json.Create_Msg_Json(NetOpCode.RoomPlayerLeave_Msg, new RoomPlayerLeave_Msg { RoomPlayer_Data = rpd.RoomPlayer_Data }));
            {
                //广播消息
                //SendToAllPlayer()
            }


            return ret;
        }

        public virtual void OpenRoom()
        {
            this.IsAvailable = false;
            Log.Debug("OpenRoom"+this.RoomId);
        }
        public virtual void CloseRoom()
        {
            this.IsAvailable = true;

            var Ls_Tpids = this.GetTablePlayerIds();
            foreach(var id in Ls_Tpids)
            {
                this.RemovePlayer(id);
            }
            Log.Debug("CloseRoom" + this.RoomId);
        }
        
        public List<string> GetTablePlayerIds()
        {
            List<string> Ls_Tpids = new List<string>();
            foreach(var vk in this.Dict_Rp)
            {
                Ls_Tpids.Add(vk.Key);
            }
            return Ls_Tpids;
        }
        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        public void CheckInActivePy()
        {
            if(this.IsAvailable)
            {
                return;
            }

            //////////////////////////////////////////////////////////////////////////删除长时间未连接的玩家
            List<string> Ls_Need_Del = new List<string>();
            foreach (var pb in m_Dict_Rp)
            {
                if(!pb.Value.IsOnline)
                {
                    Ls_Need_Del.Add(pb.Value.Id);
                }
                /*
                var ts = DateTime.Now - pb.Value.LastActiveTime;
                if ((uint)ts.TotalSeconds > GameConstant.TPlayerIdle)
                {
                    if (pb.Value.IsOnline)
                    {
                        pb.Value.CloseSocket();
                    }
                    Ls_Need_Del.Add(pb.Value.Id);
                }*/
            }
            foreach(var pbid in Ls_Need_Del)
            {
                var tbpy = this.GetPlayer(pbid);
                this.ExitRoom(tbpy);
                //this.RemovePlayer(pbid);
            }
            if(this.Dict_Rp.Count == 0)
            {
                //Log.Debug($"掉线结束房间{RoomId}");
                CloseRoom();
            }
            //this.RemovePlayer()
            //////////////////////////////////////////////////////////////////////////结束游戏删除所有在线玩家
            /*if (Ls_Need_Del.Count > 0)
            {
                //////////////////////////////////////////////////////////////////////////
                Log.Debug($"掉线结束房间{RoomId}");
                //SendGameOverMsg(2);//对手已经掉线,结束
                CloseRoom();
                //////////////////////////////////////////////////////////////////////////
            }*/

            //////////////////////////////////////////////////////////////////////////
        }

    }
}
