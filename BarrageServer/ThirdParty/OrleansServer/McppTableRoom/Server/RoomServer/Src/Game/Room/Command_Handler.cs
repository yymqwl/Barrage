using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [MessageHandler]
    public class Command_Handler : NeedInRoom_Handler<Command_Msg>
    {
        protected override void Run(WebPlayer webpy, RoomPlayerBv rpb, TableRoom tr, JObject message)
        {
            if (tr.RoomState == ERoomState.ERoom_InGame)
            {
              var command_Msg =  message.ToObject<Command_Msg>();
              tr.ServerPlayer.SendCommand(command_Msg.Command);
            }
        }
    }
}
