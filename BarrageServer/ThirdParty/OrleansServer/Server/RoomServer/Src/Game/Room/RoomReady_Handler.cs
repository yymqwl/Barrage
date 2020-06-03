using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [MessageHandler]
    public class RoomReady_Handler : NeedInRoom_Handler<RoomReady_Req>
    {
        protected override void Run(WebPlayer webpy, RoomPlayerBv rpb, TableRoom tr, JObject message)
        {
            var roomReady_Req = message.ToObject<RoomReady_Req>();
            rpb.RoomPlayer_Data.IsReady = roomReady_Req.IsReady;
            var roomReady_Res = new RoomReady_Res();
            roomReady_Res.Id = rpb.RoomPlayer_Data.Id;
            roomReady_Res.IsReady = rpb.RoomPlayer_Data.IsReady;
            rpb.StartGame_Data = roomReady_Req.StartGame_Data;
            tr.SendToAllPlayer(Msg_Json.Create_Msg_Json(NetOpCode.RoomReady_Res, roomReady_Res));
        }
    }
}
