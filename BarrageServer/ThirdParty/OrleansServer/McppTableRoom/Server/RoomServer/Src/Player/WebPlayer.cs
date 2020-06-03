using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using WebSocketSharp.Server;
using TableRoom;
using System.Threading.Tasks;

namespace RoomServer
{
    public class WebPlayer : APlayer , INetUserObserver
    {

        protected IWebSocketSession m_WebSession;
        public IWebSocketSession WebSession
        {
            get { return m_WebSession; }
            set { this.m_WebSession = value;}
        }

        public override bool IsOnline
        {
            get
            {
                if(this.m_WebSession == null ||
                  this.m_WebSession.ConnectionState != WebSocketSharp.WebSocketState.Open)
                {
                    return false;
                }
                return true;
            }
        }

        protected INetUserObserver m_INetUserObserver;

        public WebPlayer(string id)
        {
            this.m_Id = id;
            m_LastActiveTime = DateTime.Now;
            //建立连接
        }

        /// <summary>
        /// 检查需要NetUser
        /// </summary>
        /// <returns></returns>
        public async Task CheckNetUser()
        {
            var nue = GameMainEntry.Instance.SiloClientModule.INetUserEntry;
            if (! await nue.IsConnected(m_Id))
            {
                await SetNetUser();
            }
        }
        public async Task SetNetUser()
        {
            await ClearNetUser();
            var nue = await GameMainEntry.Instance.SiloClientModule.IMainEntry.GetINetUserEntry();
            m_INetUserObserver = await GameMainEntry.Instance.SiloClientModule.IClusterClient.CreateObjectReference<INetUserObserver>(this);
            await nue.CreateNetUser(m_Id, m_INetUserObserver);
        }
        public async Task ClearNetUser()
        {
            if (m_INetUserObserver != null)
            {
                await GameMainEntry.Instance.SiloClientModule.IClusterClient.DeleteObjectReference<INetUserObserver>(m_INetUserObserver);
                m_INetUserObserver = null;
            }
        }
        public override bool Init()
        {
            Log.Debug($"WebPlayer Init:{Id}");
            ClearNetUser().Wait();
            return base.Init();
        }
        public override bool ShutDown()
        {
            Log.Debug($"WebPlayer ShutDown:{Id}");
            ClearNetUser().Wait();
            return base.ShutDown();
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

        public void CloseSocket()
        {
            if (this.m_WebSession == null)
            {
                return;//没有打开
            }
            this.m_WebSession.Context.WebSocket.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void Receive(byte[] msg)
        {
            this.SendAsync(msg);
        }
        //
    }
}
