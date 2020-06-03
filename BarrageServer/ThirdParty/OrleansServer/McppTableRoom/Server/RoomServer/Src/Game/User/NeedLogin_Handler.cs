using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp.Server;

namespace RoomServer
{
    //需要阻拦的消息
    public abstract class NeedLogin_Handler<T> : AMWHandler<T> where T : class, IMessage
    {
        
        protected override void Run(IWebSocketSession session, JObject message)
        {

            var py = GameMainEntry.Instance.PlayerModule.Session_Pys.GetValueByKey(session.ID);
            if(py  == null)
            {
                this.SendReLogin(session);
                return;
            }
            this.Run(py,message);
        }

        protected abstract void Run(WebPlayer webpy, JObject message);

        protected virtual void SendReLogin(IWebSocketSession session)
        {
           var bys =  Msg_Json.Create_Msg_Json<NeedLogin_Msg>(NetOpCode.NeedLogin_Msg,new NeedLogin_Msg());
           session.Context.WebSocket.Send(bys);
        }
    }
}
