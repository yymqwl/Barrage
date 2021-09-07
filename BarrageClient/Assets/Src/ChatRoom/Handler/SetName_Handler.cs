using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using GameMain.ChatRoom;
namespace ChatRoom
{
    [MessageHandler]
    public class SetName_Handler : AMHandler<SetName_Res>
    {
        protected override void Run(Session session, SetName_Res message)
        {
            Log.Debug($"SetName{message.Result}");
        }
    }
}
