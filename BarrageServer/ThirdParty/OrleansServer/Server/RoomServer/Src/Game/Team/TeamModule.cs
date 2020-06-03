using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [GameFrameworkModule]
    public class TeamModule : GameFrameworkModule
    {

        public override int Priority
        {
            get
            {
                return 1000;
            }
        }


        protected Dictionary<string, Team> m_Dict_Team = new Dictionary<string, Team>();

        public Dictionary<string, Team> Dict_Team
        {
            get
            {
                return m_Dict_Team;
            }
        }
        



        
        public override bool Init()
        {
            var pret = base.Init();
            CreateTeam();

            return pret;
        }
        protected void CreateTeam()
        {
            this.m_Dict_Team.Clear();
            for (int i = 0; i < GameConstant.MaxPlayerNub; ++i)
            {
                var tr = new Team();
                tr.Init(i.ToString());//房间号
                this.m_Dict_Team.Add(i.ToString(), tr);
            }
        }

        public override void Update()
        {
            foreach (var vk in m_Dict_Team)
            {
                if(!vk.Value.IsAvailable)//使用中的Team进行刷新
                {
                    vk.Value.Update(ClientTimer.Instance.DeltaTime, ClientTimer.Instance.UnScaledDeltaTime);
                }
            }
        }

        /// <summary>
        /// -1房间不够
        /// -2必须先创建房间
        /// </summary>
        /// <param name="webPlayer"></param>
        /// <returns></returns>
        public int EnterTeam(WebPlayer webPlayer)
        {
            var ret = -1;
            var tpbv = webPlayer.GetIBehaviour<TeamPlayerBv>();
            if (tpbv == null)
            {
                return ret;
            }

            Team tr = null;

            if (tpbv.TeamId != string.Empty)
            {
                tr = this.GetTeam(tpbv.TeamId);
                if (tr.IsAvailable)//没人必须创建房间
                {
                    ret = -2;
                    return ret;
                }
                ret = tr.EnterTeam(webPlayer);
                return ret;
            }

            if (tpbv.TeamPlayer_Data.TeamId == string.Empty)//创建房间
            {

                /////////////////////////////////////////////////
                //创建新房间
                tr = this.GetAvailableTeam();
                if (null == tr)
                {
                    return ret;
                }

                tr.OpenTeam();//
                tr.TeamLeader = webPlayer;
                ret = tr.EnterTeam(webPlayer);
                tpbv.TeamId  = tr.TeamId;
            }
            else//加入别的房间
            {
                tr = this.GetTeam(tpbv.TeamPlayer_Data.TeamId);
                if (tr.IsAvailable)//没人必须创建房间
                {
                    ret = -2;
                    return ret;
                }
                ret = tr.EnterTeam(webPlayer);
                tpbv.TeamId = tr.TeamId;
            }
            return ret;
        }

        public int ExitRoom(WebPlayer webPlayer)
        {
            var ret = -1;
            var rpv = webPlayer.GetIBehaviour<TeamPlayerBv>();
            if (rpv == null)
            {
                return ret;
            }
            if (rpv.TeamId != string.Empty)
            {
                var tr = this.GetTeam(rpv.TeamId);
                if (tr == null)
                {
                    return ret;
                }
                if (webPlayer == tr.TeamLeader )//队长退出则解散
                {
                    tr.DisBand();
                    return ret;
                }

                ret = tr.ExitTeam(webPlayer);
                rpv.TeamId = string.Empty;
                if (0 == tr.Dict_Rp.Count)//
                {
                    tr.CloseTeam();
                }
            }

            return ret;
        }



        public Team GetTeam(string id)
        {
            Team tr;
            m_Dict_Team.TryGetValue(id, out tr);
            return tr;
        }
        public Team GetAvailableTeam()
        {
            foreach (var vk in m_Dict_Team)
            {
                if (vk.Value.IsAvailable)
                {
                    vk.Value.IsAvailable = false;
                    return vk.Value;
                }
            }
            return null;
        }


    }
}
