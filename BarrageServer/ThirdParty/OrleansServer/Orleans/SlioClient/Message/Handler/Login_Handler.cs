using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameMain.ChatRoom;
using IHall;

namespace SlioClient
{
    [MessageHandler]
    public class Login_Handler : AMHandler<Login_Res>
    {
        protected  override void Run(Session session, Login_Res message)
        {
            Log.Debug($"{typeof(Login_Res).Name}" + message.Result);
        }
    }
}
