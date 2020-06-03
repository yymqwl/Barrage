using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [MessageHandler]
    public class RoomLoading_Handler : NeedInRoom_Handler<RoomLoading_Req>
    {
        protected override void Run(WebPlayer webpy, RoomPlayerBv rpb, TableRoom tr, JObject message)
        {
            var roomLoading_Req = message.ToObject<RoomLoading_Req>();
            rpb.RoomPlayer_Data.IPercent = roomLoading_Req.IPercent;
            var roomLoading_Res = new RoomLoading_Res();
            roomLoading_Res.Id = rpb.RoomPlayer_Data.Id;
            roomLoading_Res.IPercent = rpb.RoomPlayer_Data.IPercent;
            tr.SendToAllPlayer(Msg_Json.Create_Msg_Json(NetOpCode.RoomLoading_Res, roomLoading_Res));

        }
    }
}
