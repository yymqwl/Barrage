using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using Newtonsoft.Json.Linq;


namespace RoomServer
{
    public abstract class NeedInRoom_Handler<T> : NeedLogin_Handler<T> where T : class, IMessage
    {
        protected override void Run(WebPlayer webpy, JObject message)
        {
            var rpbv = webpy.GetIBehaviour<RoomPlayerBv>();
            if (rpbv == null)
            {
                webpy.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.NeedEnterRoom_Msg, new NeedEnterRoom_Msg()));
                return;
            }
            var tr = GameMainEntry.Instance.RoomModule.GetTableRoom(rpbv.RoomId);
            if(tr == null)
            {
                Log.Debug("没有房间");
                webpy.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.NeedEnterRoom_Msg, new NeedEnterRoom_Msg()));
                return;
            }
            this.Run(webpy,rpbv,tr, message);
        }
        protected abstract void Run(WebPlayer webpy, RoomPlayerBv rpb,TableRoom tr ,JObject message);
    }
}
