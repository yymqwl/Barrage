using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    public class TeamPlayerBv : ABehaviour
    {
        protected string m_TeamId = string.Empty;//队伍的Id
        public virtual string TeamId
        {
            get
            {
                return this.m_TeamId;
            }
            set { this.m_TeamId = value; }
        }
        protected  TeamPlayer_Data m_TeamPlayer_Data;
        public  TeamPlayer_Data TeamPlayer_Data
        {
            get 
            {
                return this.m_TeamPlayer_Data;
            }
            set
            {
                this.m_TeamPlayer_Data = value;
            }
        }

    }

    public class TeamPlayer_Data
    {
        public string Id;
        public string TeamId;
        public string NickName;
        public string AvatarUrl;
        public int IStars;
        public int Camp;//阵营
        public int CampId;//几号位
    }
}
