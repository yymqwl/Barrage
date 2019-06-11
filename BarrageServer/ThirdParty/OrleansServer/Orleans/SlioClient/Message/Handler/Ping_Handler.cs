using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using IHall;
using GameMain.ChatRoom;


namespace SlioClient
{
    [MessageHandler]
    public class Ping_Handler : AMHandler<Ping_Msg>
    {
        protected override void Run(Session session, Ping_Msg message)
        {
            Log.Debug($"Ping:{message.Time}");
        }
    }
}
