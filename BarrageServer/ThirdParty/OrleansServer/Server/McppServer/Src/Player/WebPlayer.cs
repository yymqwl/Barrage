using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using WebSocketSharp.Server;

namespace Mcpp
{
    public class WebPlayer : APlayer
    {

        protected JObject m_User_Data;
        protected IWebSocketSession m_WebSession;
        public IWebSocketSession WebSession
        {
            get { return m_WebSession; }
            set { this.m_WebSession = value;}
        }
        public JObject User_Data
        {
            get
            {
                return this.m_User_Data;
            }
        }
        public WebPlayer(string id)
        {
            this.m_Id = id;
            m_LastActiveTime = DateTime.Now;
        }
        public override bool Init()
        {

            var db = GameMainEntry.Instance.DbModule.Db;

            var rd_data = db.HashGet(GameConstant.Str_User_Data_Tb, this.Id);
            if(rd_data.IsNull)
            {
                m_User_Data = new JObject();
                db.HashSet(GameConstant.Str_User_Data_Tb, this.Id, JsonHelper.JsonSerialize_String(m_User_Data));
            }
            else
            {
                m_User_Data = JObject.Parse(rd_data );
            }
            return base.Init();
        }
        public void SendAsync(byte[] data)
        {
            if(this.m_WebSession == null||
                this.m_WebSession.ConnectionState != WebSocketSharp.WebSocketState.Open)
            {
                return;//没有打开
            }
            this.m_WebSession.Context.WebSocket.Send(data);
        }
        public void SaveData()
        {
            var db = GameMainEntry.Instance.DbModule.Db;
            db.HashSet(GameConstant.Str_User_Data_Tb, this.Id, JsonHelper.JsonSerialize_String(m_User_Data));
        }
    }
}
