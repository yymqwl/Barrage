using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [MessageHandler]
    public class RoomReadyFinish_Handler : NeedInRoom_Handler<RoomReadyFinish_Req>
    {
        protected override void Run(WebPlayer webpy, RoomPlayerBv rpb, TableRoom tr, JObject message)
        {
            rpb.StartGame_Data = message;
        }
    }
}
