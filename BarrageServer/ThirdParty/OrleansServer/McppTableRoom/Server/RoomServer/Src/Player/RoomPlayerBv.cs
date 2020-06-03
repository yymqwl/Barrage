using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    public class RoomPlayerBv : ABehaviour
    {

        protected string m_RoomId = string.Empty;//所在的房间Id
        public virtual string RoomId
        {
            get
            {
                return this.m_RoomId;
            }
            set { this.m_RoomId = value; }
        }
        protected ERoomPlayerState m_RoomPlayerState;
        public ERoomPlayerState RoomPlayerState
        {
            get { return m_RoomPlayerState; }
            set { this.m_RoomPlayerState = value; }
        }
        protected RoomPlayer_Data m_RoomPlayer_Data;
        public RoomPlayer_Data RoomPlayer_Data
        {
            get
            {
               return this.m_RoomPlayer_Data;
            }
            set
            {
                this.m_RoomPlayer_Data = value;
            }
        }
        protected JObject m_StartGame_Data;
        public JObject StartGame_Data
        {
            get 
            {
                return m_StartGame_Data;
            }
            set
            {
                this.m_StartGame_Data = value;
            }
        }

        public void RestReady()
        {
            m_RoomPlayer_Data.IsReady = false;
            m_RoomPlayer_Data.IPercent = 0;
            m_StartGame_Data = null;
            m_RoomPlayerState = ERoomPlayerState.E_Ready;
        }

    }
    public class RoomPlayer_Data
    {
        public string Id;
        public string RoomId;
        public string NickName;
        public string AvatarUrl;
        public bool IsReady;
        public int  IPercent;
        public int  Camp;//阵营 
        public int  CampId;//小队编号
    }


    public enum ERoomPlayerState
    {
        E_Ready,
        E_Loading,
        E_InGame,
        E_GameOver,
    }
}
