using GameFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer
{
    [MessageHandler]
    public class GameOver_Handler : NeedInRoom_Handler<GameOver_Req>
    {
        protected override void Run(WebPlayer webpy, RoomPlayerBv rpb, TableRoom tr, JObject message)
        {
            rpb.RoomPlayerState = ERoomPlayerState.E_GameOver;
            tr.ChangeToGameOver();
        }
    }
}
