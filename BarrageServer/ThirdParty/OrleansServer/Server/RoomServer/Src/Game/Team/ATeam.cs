using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    public abstract class ATeam
    {

        protected string m_TeamId;
        public virtual string TeamId
        {
            get
            {
                return this.m_TeamId;
            }
        }

        protected WebPlayer m_TeamLeader;
        public  WebPlayer TeamLeader
        {
            get
            {
                return this.m_TeamLeader;
            }
            set
            {
                this.m_TeamLeader = value;
            }
        }
        bool m_IsAvailable;
        public bool IsAvailable
        {
            get { return m_IsAvailable; }
            set { m_IsAvailable = value; }
        }

        protected Dictionary<string, WebPlayer> m_Dict_Rp = new Dictionary<string, WebPlayer>();
        public Dictionary<string, WebPlayer> Dict_Rp
        {
            get
            {
                return m_Dict_Rp;
            }
        }
        public virtual bool Init(string teamid)
        {
            this.m_TeamId = teamid;
            this.m_IsAvailable = true;
            return true;
        }
        public virtual bool ShutDown()
        {
            this.m_Dict_Rp.Clear();
            return true;
        }
        public virtual bool HasPlayer(string id)
        {
            if (this.m_Dict_Rp.ContainsKey(id))
            {
                return true;
            }
            return false;
        }
        public WebPlayer GetPlayer(string id)
        {
            WebPlayer rp;
            this.m_Dict_Rp.TryGetValue(id, out rp);
            return rp;
        }
        public bool RemovePlayer(string id)
        {
            var wp = GetPlayer(id);
            if (wp != null)
            {
                var tpb = wp.GetIBehaviour<TeamPlayerBv>();
                tpb.TeamId = string.Empty;
            }
            return this.m_Dict_Rp.Remove(id);
        }

        public void SendToAllPlayer(byte[] data)
        {
            foreach (var rp in m_Dict_Rp)
            {
                rp.Value.SendAsync(data);
            }
        }

        public int EnterTeam(WebPlayer webPlayer)
        {
            var ret = -1;
            var py = this.GetPlayer(webPlayer.Id);
            if (py != null)
            {
                ret = 2;//已经在房间中
                return ret;
            }
            //广播加入
            var tpb = webPlayer.GetIBehaviour<TeamPlayerBv>();
            tpb.TeamId = this.TeamId;
            tpb.TeamPlayer_Data.TeamId = this.TeamId;

            SendToAllPlayer(Msg_Json.Create_Msg_Json(NetOpCode.TeamPlayerJoin_Msg, new TeamPlayerJoin_Msg { TeamPlayer_Data = tpb.TeamPlayer_Data }));


            Dict_Rp.Add(webPlayer.Id, webPlayer);
            Log.Debug($"{webPlayer.Id}加入队伍:{TeamId}");

            ret = 1;
            return ret;
        }

        public int ExitTeam(WebPlayer webPlayer)
        {
            var ret = -1;
            var py = this.GetPlayer(webPlayer.Id);
            if (py == null)
            {
                return ret;
            }
            ret = 1;
            RemovePlayer(webPlayer.Id);
            Log.Debug($"{webPlayer.Id}退出队伍:{TeamId}");
            var tpb = webPlayer.GetIBehaviour<TeamPlayerBv>();
            SendToAllPlayer(Msg_Json.Create_Msg_Json(NetOpCode.TeamPlayerLeave_Msg, new TeamPlayerLeave_Msg { TeamPlayer_Data = tpb.TeamPlayer_Data }));
            return ret;
        }

        //解散小队
        public void DisBand()
        {
            foreach(var vk in this.Dict_Rp)
            {
                vk.Value.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.TeamDisband_Msg,new TeamDisband_Msg()));
            }
            this.CloseTeam();
        }
        public virtual void OpenTeam()
        {
            this.IsAvailable = false;
        }
        public virtual void CloseTeam()
        {
            this.IsAvailable = true;
            var Ls_Tpids = this.GetTablePlayerIds();
            foreach (var id in Ls_Tpids)
            {
                this.RemovePlayer(id);
            }
            Log.Debug("解散Team" + this.TeamId);
        }
        public List<string> GetTablePlayerIds()
        {
            List<string> Ls_Tpids = new List<string>();
            foreach (var vk in this.Dict_Rp)
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
            if (this.IsAvailable)
            {
                return;
            }

            //////////////////////////////////////////////////////////////////////////删除长时间未连接的玩家
            List<string> Ls_Need_Del = new List<string>();
            foreach (var pb in m_Dict_Rp)
            {
                if (!pb.Value.IsOnline)
                {
                    Ls_Need_Del.Add(pb.Value.Id);
                }
            }
            foreach (var pbid in Ls_Need_Del)
            {
                var tbpy = this.GetPlayer(pbid);
                this.ExitTeam(tbpy);
            }
            if (this.Dict_Rp.Count == 0)
            {
                CloseTeam();
            }
            //////////////////////////////////////////////////////////////////////////
        }
    }
}
