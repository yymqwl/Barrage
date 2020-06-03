using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Threading;
using Loger =  GameFramework.Log;
using GameFramework;
using Newtonsoft.Json.Linq;

namespace RoomServer
{
    public class WebBv: WebSocketBehavior
    {
        protected override void OnOpen()
        {
            
            //Loger.Debug(this.ID+"OnOpen"+Thread.CurrentThread.ManagedThreadId);
            base.OnOpen();
        }
        protected override void OnClose(CloseEventArgs e)
        {
            //Loger.Debug("OnClose"+Thread.CurrentThread.ManagedThreadId);
            base.OnClose(e);
        }
        protected override void OnError(ErrorEventArgs e)
        {
            Loger.Debug("OnError"+e.Message); 
            base.OnError(e);
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            //Loger.Debug(ID+ $"OnMessage{e.Data.Length}:" + e.Data);
            //Loger.Debug(LZString.Decompress(e.Data));
            try
            {
                var Json_Data = LZString.DecompressFromUint8Array(e.RawData);
                var MsgBody = JObject.Parse(Json_Data);
                var id = MsgBody.GetValue("Id").Value<ushort>();
                JObject data= MsgBody.GetValue("Data").Value<JObject>();

                IWebSocketSession session;
                this.Sessions.TryGetSession(ID, out session);


                //GameMainEntry.Instance.WebServerModule.Dispather.Dispatch(session, new MessageInfo_Json(id, data));
                
                OneThreadSynchronizationContext.Instance.Post((obj) =>
                {
                    GameMainEntry.Instance.WebServerModule.Dispather.Dispatch(session, new MessageInfo_Json(id, data));//拉到主线程处理
                }, null);
                
            }
            catch(Exception ex)
            {
                Loger.Error($"{ID} OnMessage:"+ex.ToString());
            }
            base.OnMessage(e);
        }
    }
}
