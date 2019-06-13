using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameMain.ChatRoom;


namespace ChatRoom
{
    [MessageHandler]
    public class Ping_Handler : AMHandler<Ping_Msg>
    {
        protected override void Run(Session session, Ping_Msg message)
        {
            var sp = new TimeSpan(DateTime.UtcNow.Ticks - message.Time);
            Log.Debug($"Ping:{sp.TotalMilliseconds}");
        }
    }
}
