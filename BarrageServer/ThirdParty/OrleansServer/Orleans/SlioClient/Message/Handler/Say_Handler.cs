using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameMain.ChatRoom;
using IHall;
namespace SlioClient
{
    [MessageHandler]
    public class Say_Handler : AMHandler<Say_Res>
    {
        protected override void Run(Session session, Say_Res message)
        {

            Log.Debug($"{message.Msg}");
        }
    }
}
