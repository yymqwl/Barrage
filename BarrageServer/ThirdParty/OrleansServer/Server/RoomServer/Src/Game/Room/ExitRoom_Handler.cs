using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [MessageHandler]
    public class ExitRoom_Handler : NeedInRoom_Handler<ExitRoom_Req>
    {
        protected override void Run(WebPlayer webpy, RoomPlayerBv rpb, TableRoom tr ,  JObject message)
        {
            var exitRoom_Res = new ExitRoom_Res() { Res = -1 };
            var exitRoom_Req = message.ToObject<ExitRoom_Req>();
            var rpbv = webpy.GetIBehaviour<RoomPlayerBv>();
            exitRoom_Res.Res =  GameMainEntry.Instance.RoomModule.ExitRoom(webpy);
            webpy.SendAsync(Msg_Json.Create_Msg_Json(NetOpCode.ExitRoom_Res, exitRoom_Res));
        }
    }
}
